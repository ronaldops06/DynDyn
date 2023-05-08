import os
import sys
from django.contrib import admin
from django import forms
import datetime
from rangefilter.filters import DateRangeFilter, DateTimeRangeFilter
from admin_totals.admin import ModelAdminTotals
from django.db.models import Sum, Min, Max, Q
from django.db.models.functions import Coalesce
from django.utils.html import format_html

from .models import Codes, Categorias, Contas, Operacoes, Movimentos, Saldos, Execucoes, SaldoSimulado, ContasOperacoes, SaldoProxy, Ativos, TiposAtivos

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
            (3, 'Transferência'),
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
    
    salario = forms.ChoiceField(
        label = 'Salário',
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
    
    executa_saldo_sim = forms.ChoiceField(
        label = 'Executar Saldo Simulado',
        choices = [
            (1, 'Sim'),
            (0, 'Não'),
        ],
        initial = 1,
        widget = forms.RadioSelect,
    )

class TipoContaOperacaoForm(forms.ModelForm):
        
    tipo = forms.ChoiceField(
        label = 'Tipo',
        choices = [
            (1, 'Positivo'),
            (2, 'Negativo'),
        ],
        initial = 1,
        widget = forms.RadioSelect,
    )
    
class AjustaSaldoSimuladoForm(forms.ModelForm):
        
    ajusta = forms.ChoiceField(
        label = 'Ajusta',
        choices = [
            (1, 'Sim'),
            (0, 'Não'),
        ],
        initial = 1,
        widget = forms.RadioSelect,
    )
    
class SituacaoAtivoForm(forms.ModelForm):
        
    situacao = forms.ChoiceField(
        label = 'Situacao',
        choices = [
            (1, 'Vendido'),
            (0, 'Em aberto'),
        ],
        initial = 0,
        widget = forms.RadioSelect,
    )

class MovimentosForm(forms.ModelForm):
    consolidado = forms.ChoiceField(
        label = 'Consolidado',
        choices = [
            (0, 'Não'),
            (1, 'Sim')
        ],
        initial = 0,
        widget = forms.RadioSelect
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
    search_fields = ['nome']
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
    search_fields = ['nome', 'cat_id__nome', 'conta_pai__nome']
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
    
    def salario_operacao(self, obj):
        return "Sim" if obj.salario == 1 else "Não"
        
    salario_operacao.short_description = 'Salário' 
    
    list_display = ('nome', 'cat_id', 'tipo_operacao', 'recorrente_operacao', 'status_operacao', 'salario_operacao')
    search_fields = ['nome', 'cat_id__nome', 'tipo_operacao']
    form = TipoStatusOperacaoForm
    fields = ['nome', ('tipo', 'recorrente'), 'cat_id', 'status', 'salario']
        
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

    def conta_destino(self, obj):
        return obj

    conta_destino.short_description = 'Conta Destino'

    def consolidado_color(self, obj):
        text = 'Não'
        color = 'red'
        
        if obj.consolidado == 1:
            text = 'Sim'
            color = 'green'
        return format_html('<b style="color:{};">{}</b>',color,text,obj.consolidado)
    
    consolidado_color.short_description = 'Consolidado'
        
    list_display = ('ope_id', 'consolidado_color', 'cta_id', 'conta_destino', 'valor', 'observacao', 'data_criacao', 'valor_convertido')
    search_fields = ['ope_id__nome']
    #list_editable = ('consolidado',)
    date_hierarchy = 'dt_criacao'
    #list_totals = [('valor_convertido', lambda function: Sum('valor_convertido'))]
    list_filter = (
        ('dt_criacao', DateTimeRangeFilter),
        ('cta_id__nome'),
    )
    form = MovimentosForm
    fields = ['consolidado', 'ope_id', 'cta_id', 'conta_destino', 'valor', 'observacao', 'parcela', 'total_parcelas', 'dt_criacao']
    
@admin.register(TiposAtivos)
class TiposAtivosAdmin(admin.ModelAdmin):

    list_display = ('codigo', 'descricao', 'conta')
    fields = ['codigo', 'descricao', 'conta']

@admin.register(Ativos)
class AtivosAdmin(admin.ModelAdmin):

    def situacao(self, obj):
        return "Vendido" if obj.situacao == 1 else "Em aberto"
    
    situacao.short_description = 'Situação' 
    
    def data_criacao(self, obj):
        return obj.dt_criacao.strftime("%d/%m/%Y %H:%M:%S")
        
    data_criacao.short_description = 'Data Criação' 
    
    def data_vencimento(self, obj):
        return obj.dt_vencimento.strftime("%d/%m/%Y %H:%M:%S") if obj.dt_vencimento != None else None
        
    data_vencimento.short_description = 'Data Vencimento' 
    
    def irpf_vlr_and_perc(self, obj):
        return str(obj.vlr_irpf) + ' (' + str(obj.perc_irpf) + '%)' if obj.vlr_irpf != None else 0
        
    irpf_vlr_and_perc.short_description = 'IR'
    
    def percentual_retorno_liq(self, obj):
        return obj.perc_retorno_liq
        
    percentual_retorno_liq.short_description = '% a.a.'
    
    def percentual_retorno_liq_mes(self, obj):
        return round(obj.perc_retorno_liq / 12, 2)
        
    percentual_retorno_liq_mes.short_description = '% a.m.'
    
    def periodo(self, obj):
        return obj.periodo_meses
        
    periodo.short_description = 'Meses'
    
    def vlr_bruto(self, obj):
        return obj.vlr_retorno_bruto
        
    vlr_bruto.short_description = 'Vlr Bruto'
    
    def vlr_liquido(self, obj):
        return obj.vlr_retorno_liq
        
    vlr_liquido.short_description = 'Vlr Liq.'
        
    list_display = ('nome', 'vlr_compra', 'vlr_venda', 'data_criacao', 'data_vencimento', 'periodo', 'vlr_bruto', 'vlr_liquido', 'percentual_retorno_liq', 'percentual_retorno_liq_mes', 'irpf_vlr_and_perc', 'vlr_despesas' )
    search_fields = ['nome']
    readonly_fields = ['periodo_meses', 'vlr_retorno_bruto', 'vlr_retorno_liq', 'perc_retorno_liq', 'perc_irpf', 'vlr_irpf']
    form = SituacaoAtivoForm
    fields = ['situacao', 'tipo_ativo', 'nome', ('vlr_compra', 'vlr_venda'), ('dt_criacao', 'dt_vencimento'), 'vlr_despesas']
 
@admin.register(Saldos)
class SaldosAdmin(admin.ModelAdmin):
	def get_queryset(self, request):
		qs = super(SaldosAdmin, self).get_queryset(request)
		return qs.filter((Q(cta_id__status=0) & Q(vlr_acumulado__gt=0)) | Q(cta_id__status=1))
		
	def get_rangefilter_dt_saldo_default(self, request):
		return (datetime.datetime.today, datetime.datetime.today)

	def get_rangefilter_dt_saldo_title(self, request, field_path):
		return 'Data do Saldo'
        
	def data_saldo(self, obj):
		return obj.dt_saldo.strftime("%d/%m/%Y %H:%M:%S")
        
	data_saldo.short_description = 'Data Saldo' 
    
	list_display = ('cta_id', 'data_saldo', 'valor', 'vlr_acumulado', 'vlr_valorizacao', 'vlr_dividendo', 'vlr_rendimento', 'credito', 'debito', 'credito_salario', 'debito_salario')
    search_fields = ['cta_id__nome']
	date_hierarchy = 'dt_saldo'
	list_filter = (
		('dt_saldo', DateTimeRangeFilter),
	)
	fields = ['cta_id', 'dt_saldo', 'valor', 'vlr_acumulado', ('vlr_valorizacao', 'vlr_dividendo', 'vlr_rendimento'), ('credito', 'debito'), ('credito_salario', 'debito_salario')]

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
        
    def exec_saldo_sim(self, obj):
        return "Ativa" if obj.executa_saldo_sim == 1 else "Inativa"
    
    exec_recorrencia.short_description = 'Execução Saldo Simulado' 
    
    def data_exec_saldo_sim(self, obj):
        return obj.dt_exec_saldo_sim.strftime("%d/%m/%Y") if obj.dt_exec_saldo_sim != None else ''
                
    data_exec_rec.short_description = 'Data Execução Saldo Simulado' 
    
    list_display = ('exec_saldo', 'data_exec_saldo', 'exec_recorrencia', 'data_exec_rec', 'exec_saldo_sim', 'data_exec_saldo_sim')
    form = ExecutaExecucoesForm
    fields = [('executa_saldo', 'dt_exec_saldo'), ('executa_recorrencia', 'dt_exec_rec'), ('executa_saldo_sim', 'dt_exec_saldo_sim')]
    
@admin.register(SaldoSimulado)
class SaldoSimuladoAdmin(ModelAdminTotals):

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
    
    def ajusta_saldo(self, obj):
        return "Sim" if obj.ajusta == 1 else "Não"
    
    ajusta_saldo.short_description = 'Ajusta Saldo' 
        
    list_display = ('saldo', 'vlr_acumulado_saldo', 'vlr_real', 'vlr_diferenca', 'ajusta_saldo', 'ajusta', 'data_saldo')
    list_editable = ('vlr_real', 'ajusta')
    list_totals = [('vlr_real', lambda function: Sum('vlr_real'))]
    list_filter = (
        ('dt_criacao', DateTimeRangeFilter),
    )
    form = AjustaSaldoSimuladoForm
    fields = ['saldo', 'vlr_real', 'vlr_diferenca', 'ajusta']
    
@admin.register(ContasOperacoes)
class ContasOperacoesAdmin(admin.ModelAdmin):
    
    def tipo_conta_operacao(self, obj):
        return "Positivo" if obj.tipo == 1 else "Negativo"
    
    tipo_conta_operacao.short_description = 'Tipo' 

    def data_base(self, obj):
        return obj.dt_base.strftime("%d/%m/%Y")
                
    data_base.short_description = 'Data base movimento' 
    
    list_display = ('conta', 'operacao', 'tipo_conta_operacao', 'data_base')
    form = TipoContaOperacaoForm
    fields = ['conta', 'operacao', 'tipo', 'dt_base']
    
@admin.register(SaldoProxy)
class DashboardAdmin(admin.ModelAdmin):

    def changelist_view(self, request, extra_context=None):
        response = super().changelist_view(
            request,
            extra_context=extra_context,
        )

        try:
            qs = response.context_data['cl'].queryset
        except (AttributeError, KeyError):
            return response
            
        summary_over_time = qs.filter(cta_id__nome='Geral').order_by('dt_saldo')

        summary_range = summary_over_time.aggregate(
            low=Min('vlr_acumulado'),
            high=Max('vlr_acumulado'),
        )
        high = summary_range.get('high', 0)
        low = summary_range.get('low', 0)
        
        saldos = list(summary_over_time)
        v_vlr_estimado = 0
        for saldo in saldos:
            saldo.low = low
            saldo.high = high
            saldo.pct = ((saldo.vlr_acumulado or 0) - low) / (high - low) * 100
            if v_vlr_estimado == 0:
                v_vlr_estimado = saldo.vlr_acumulado
                saldo.vlr_estimado = v_vlr_estimado
            else:
                v_vlr_estimado = v_vlr_estimado + (v_vlr_estimado * 0.04)
                saldo.vlr_estimado = v_vlr_estimado
            
        response.context_data['saldos'] = saldos
        
        # rendimentos
        rendimentos = qs.filter(Q(vlr_dividendo__gt=0) | Q(vlr_rendimento__gt=0)).values('dt_saldo').order_by('dt_saldo').annotate(vlr_saldo_conta=Coalesce(Sum('vlr_acumulado'),0.0), valor=Coalesce(Sum('vlr_dividendo'),0.0) + Coalesce(Sum('vlr_rendimento'),0.0))
        v_qtde_meses = 1
        v_prc_medio_rend = 0
        vlr_saldo_mes_anterior = 0
        for rendimento in rendimentos:
            rendimento['color'] = 'blue' if rendimento['valor'] >= 0 else 'red'
            rendimento['vlr_total'] = qs.filter(cta_id__nome='Geral').filter(dt_saldo=rendimento['dt_saldo']).first().vlr_acumulado
            rendimento['prc_rendimento'] = (rendimento['valor'] * 100) / vlr_saldo_mes_anterior if vlr_saldo_mes_anterior > 0 else 0
            rendimento['prc_medio_rend'] = v_prc_medio_rend
            v_qtde_meses += 1
            v_prc_medio_rend += rendimento['prc_rendimento']
            vlr_saldo_mes_anterior = rendimento['vlr_saldo_conta']
        
        for rendimento in rendimentos:
            rendimento['prc_medio_rend'] = v_prc_medio_rend / v_qtde_meses
            
        response.context_data['rendimentos'] = rendimentos

        return response
        
    change_list_template = 'admin/prog_saldos_change_list.html'

class Config():
    def get_app_list(self, request):
        """
        Return a sorted list of all the installed apps that have been
        registered on this site.
        """
        ordering = {
            # for superuser
            'Group': 1,
            'User': 2,

            # fist app
            'SaldoProxy': 101,
            'Movimentos': 102,
            'Ativos': 103,
            'Saldos': 104,
            'SaldoSimulado': 105,
            'Categorias': 106,
            'Contas': 107,
            'Operacoes': 108,
            'ContasOperacoes': 109,
            'TiposAtivos': 110,
            'Execucoes': 111,
            'Codes': 112,
            
        }
        app_dict = self._build_app_dict(request)
        app_list = sorted(app_dict.values(), key=lambda x: x['name'].lower())

        for app in app_list:
            app['models'].sort(key=lambda x: ordering[x['object_name']])

        return app_list
    
admin.AdminSite.get_app_list = Config.get_app_list