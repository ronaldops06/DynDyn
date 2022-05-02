import os
import sys
from django.contrib import admin
from django import forms
import datetime
from rangefilter.filters import DateRangeFilter, DateTimeRangeFilter
from admin_totals.admin import ModelAdminTotals
from django.db.models import Sum, Avg
from django.db.models.functions import Coalesce

from .models import Codes, Categorias, Contas, Operacoes, Movimentos, Saldos, Execucoes, SaldoSimulado

# Register your models here.
#admin.site.register(Codes)
#admin.site.register(Categorias)
#admin.site.register(Contas)
#admin.site.register(Operacoes)
#admin.site.register(Movimentos)
#admin.site.register(Saldos)
    
class TipoStatusCategoriaForm(forms.ModelForm):
    class Meta:
        model = Codes
        fields = '__all__'
        
    tipo = forms.ChoiceField(
        label = 'Tipo',
        choices = [
            (1, 'Conta'),
            (2, 'Operação'),
        ],
        initial = 1,
        widget = forms.RadioSelect,
    )
    
    status = forms.ChoiceField(
        label = 'Status',
        choices = [
            (1, 'Ativo'),
            (0, 'Inativo'),
        ],
        initial = 1,
        widget = forms.RadioSelect,
    )

class TipoStatusOperacaoForm(forms.ModelForm):

    class Meta:
        model = Categorias
        fields = '__all__'
        
    def __init__(self, *args, **kwargs):
        super(TipoStatusOperacaoForm, self).__init__(*args, **kwargs)
        self.fields['cat_id'].queryset = Categorias.objects.filter(tipo=2)
        
    tipo = forms.ChoiceField(
        label = 'Tipo',
        choices = [
            (1, 'Crédito'),
            (2, 'Débito'),
        ],
        initial = 1,
        widget = forms.RadioSelect,
    )
    
    status = forms.ChoiceField(
        label = 'Status',
        choices = [
            (1, 'Ativo'),
            (0, 'Inativo'),
        ],
        initial = 1,
        widget = forms.RadioSelect,
    )
    
    recorrente = forms.ChoiceField(
        label = 'Recorrente',
        choices = [
            (1, 'Sim'),
            (0, 'Não'),
        ],
        initial = 0,
        widget = forms.RadioSelect,
    )
    
class CategoriaStatusOperacaoForm(forms.ModelForm):
    class Meta:
        model = Categorias
        fields = '__all__'
    
    def __init__(self, *args, **kwargs):
        super(CategoriaStatusOperacaoForm, self).__init__(*args, **kwargs)
        self.fields['cat_id'].queryset = Categorias.objects.filter(tipo=1)
        
    status = forms.ChoiceField(
        label = 'Status',
        choices = [
            (1, 'Ativo'),
            (0, 'Inativo'),
        ],
        initial = 1,
        widget = forms.RadioSelect,
    )
    
class ExecutaExecucoesForm(forms.ModelForm):
        
    executa_saldo = forms.ChoiceField(
        label = 'Executar Saldo',
        choices = [
            (1, 'Sim'),
            (0, 'Não'),
        ],
        initial = 1,
        widget = forms.RadioSelect,
    )
    
    executa_recorrencia = forms.ChoiceField(
        label = 'Executar Recorrência',
        choices = [
            (1, 'Sim'),
            (0, 'Não'),
        ],
        initial = 1,
        widget = forms.RadioSelect,
    )

##-------------------------------------------------------------------------##
@admin.register(Codes)
class CodesAdmin(admin.ModelAdmin):
    list_display = ('codigo', 'descricao', 'dominio')
    fields = ['codigo', 'descricao', 'dominio']
        
@admin.register(Categorias) 
class CategoriasAdmin(admin.ModelAdmin): 

    def status_categoria(self, obj):
        return "Ativo" if obj.status == 1 else "Inativo"
    
    status_categoria.short_description = 'Status' 
    
    def tipo_categoria(self, obj):
        return "Conta" if obj.tipo == 1 else "Operação"
    
    tipo_categoria.short_description = 'Tipo' 
    
    list_display = ('nome', 'tipo_categoria', 'status_categoria')
    form = TipoStatusCategoriaForm
    fields = ['nome', 'tipo', 'status']

@admin.register(Contas)
class ContasAdmin(admin.ModelAdmin):

    def status_conta(self, obj):
        return "Ativo" if obj.status == 1 else "Inativo"
    
    status_conta.short_description = 'Status' 
    
    def conta_pai(self, obj):
        return obj.conta
    
    conta_pai.short_description = 'Conta Principal' 

    list_display = ('nome', 'cat_id', 'conta_pai', 'status_conta')
    form = CategoriaStatusOperacaoForm
    fields = ['nome', 'cat_id', 'conta', 'status']
    
@admin.register(Operacoes)
class OperacoesAdmin(admin.ModelAdmin):

    def status_operacao(self, obj):
        return "Ativo" if obj.status == 1 else "Inativo"
    
    status_operacao.short_description = 'Status' 
    
    def recorrente_operacao(self, obj):
        return "Sim" if obj.recorrente == 1 else "Não"
    
    recorrente_operacao.short_description = 'Ind. Recorrente' 

    def tipo_operacao(self, obj):
        return "Crédito" if obj.tipo == 1 else "Débito"
    
    tipo_operacao.short_description = 'Tipo' 
    
    list_display = ('nome', 'cat_id', 'tipo_operacao', 'recorrente_operacao', 'status_operacao')
    form = TipoStatusOperacaoForm
    fields = ['nome', ('tipo', 'recorrente'), 'cat_id', 'status']
        
@admin.register(Movimentos)
class MovimentosAdmin(ModelAdminTotals):
            
    def get_rangefilter_dt_criacao_default(self, request):
        return (datetime.datetime.today, datetime.datetime.today)

    def get_rangefilter_dt_criacao_title(self, request, field_path):
        return 'Data de Criação'
        
    def data_criacao(self, obj):
        return obj.dt_criacao.strftime("%d/%m/%Y %H:%M:%S")
        
    data_criacao.short_description = 'Data Criação' 

    def _valor(self, obj):
        return obj.valor if obj.ope_id.tipo == 1 else obj.valor * -1
    
    _valor.short_description = 'Valor' 
    
    list_display = ('ope_id', 'cta_id', 'valor', 'observacao', 'data_criacao')
    list_totals = [('valor', lambda field: Coalesce(Sum('valor'), 0.0))]
    list_filter = (
        ('dt_criacao', DateTimeRangeFilter),
        ('cta_id__nome'),
    )
    fields = ['ope_id', 'cta_id', 'valor', 'observacao', 'dt_criacao', 'dt_alteracao']
        
@admin.register(Saldos)
class SaldosAdmin(admin.ModelAdmin):

    def get_rangefilter_dt_saldo_default(self, request):
        return (datetime.datetime.today, datetime.datetime.today)

    def get_rangefilter_dt_saldo_title(self, request, field_path):
        return 'Data do Saldo'
        
    def data_saldo(self, obj):
        return obj.dt_saldo.strftime("%d/%m/%Y %H:%M:%S")
        
    data_saldo.short_description = 'Data Saldo' 
    
    list_display = ('cta_id', 'valor', 'vlr_acumulado', 'vlr_valorizacao', 'vlr_dividendo', 'vlr_rendimento', 'credito', 'debito', 'data_saldo')
    list_filter = (
        ('dt_saldo', DateTimeRangeFilter),
    )
    fields = ['cta_id', 'dt_saldo', 'valor', 'vlr_acumulado', ('vlr_valorizacao', 'vlr_dividendo', 'vlr_rendimento'), ('credito', 'debito')]
    
@admin.register(Execucoes)
class ExecucoesAdmin(admin.ModelAdmin):

    def exec_saldo(self, obj):
        return "Ativa" if obj.executa_saldo == 1 else "Inativa"
    
    exec_saldo.short_description = 'Execução Saldo' 
        
    def data_exec_saldo(self, obj):
        return obj.dt_exec_saldo.strftime("%d/%m/%Y") if obj.dt_exec_saldo != None else ''
                
    data_exec_saldo.short_description = 'Data Execução do Saldo' 
    
    def exec_recorrencia(self, obj):
        return "Ativa" if obj.executa_recorrencia == 1 else "Inativa"
    
    exec_recorrencia.short_description = 'Execução Saldo' 
    
    def data_exec_rec(self, obj):
        return obj.dt_exec_rec.strftime("%d/%m/%Y") if obj.dt_exec_rec != None else ''
                
    data_exec_rec.short_description = 'Data Execução da Recorrência' 
    
    list_display = ('exec_saldo', 'data_exec_saldo', 'exec_recorrencia', 'data_exec_rec')
    form = ExecutaExecucoesForm
    fields = [('executa_saldo', 'dt_exec_saldo'), ('executa_recorrencia', 'dt_exec_rec')]
    
@admin.register(SaldoSimulado)
class SaldoSimuladoAdmin(admin.ModelAdmin):

    def get_rangefilter_dt_criacao_default(self, request):
        return (datetime.datetime.today, datetime.datetime.today)

    def get_rangefilter_dt_criacao_title(self, request, field_path):
        return 'Data do Saldo'
        
    def data_saldo(self, obj):
        return obj.saldo.dt_saldo.strftime("%d/%m/%Y")
                
    data_saldo.short_description = 'Data do Saldo' 
    
    def vlr_acumulado_saldo(self, obj):
        return obj.saldo.vlr_acumulado
                
    vlr_acumulado_saldo.short_description = 'Vlr. Saldo' 
    
    def valor_diferenca(self, obj):
        return '%.2f' % (obj.saldo.vlr_acumulado - obj.vlr_real)
        
    valor_diferenca.short_description = 'Vlr. Diferença' 
    
    list_display = ('saldo', 'vlr_acumulado_saldo', 'vlr_real', 'valor_diferenca', 'data_saldo')
    list_filter = (
        ('dt_criacao', DateTimeRangeFilter),
    )
    fields = [('saldo', 'vlr_real', 'vlr_diferenca')]