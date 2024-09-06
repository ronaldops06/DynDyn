from django.db import models
from django.utils import timezone
from django.db.models import F
from django.db.models.functions import Coalesce

class SimNao(models.IntegerChoices):
    FALSE = 0, 'Não'
    TRUE = 1, 'Sim'

class TipoOperacao(models.IntegerChoices):
    CREDITO = 1, 'Crédito'
    DEBITO = 2, 'Débito'
    TRANSFERENCIA = 3, 'Transferência'

# Create your models here.
class Codes(models.Model):
    id = models.AutoField(primary_key=True)
    dominio = models.CharField(max_length=30, blank=True, null=True)
    codigo = models.CharField(max_length=20, blank=True, null=True)
    descricao = models.CharField(max_length=100, blank=True, null=True)
    dt_criacao = models.DateTimeField(default=timezone.now)
    dt_alteracao = models.DateTimeField(blank=True, null=True)
    
    def __str__(self):
        return "(" + self.dominio + ")" + self.codigo + " - " + self.descricao

class Categorias(models.Model):
    id = models.AutoField(primary_key=True)
    nome = models.CharField(max_length=200, blank=True, null=True)
    tipo = models.IntegerField( blank=True, null=True)
    status = models.IntegerField( blank=True, null=True)
    dt_criacao = models.DateTimeField(default=timezone.now)
    dt_alteracao = models.DateTimeField(blank=True, null=True)
    
    def publish(self):
        self.dt_alteracao = timezone.now()
        self.save()

    def __str__(self):
        return self.nome + " (" + ("Conta" if self.tipo == 1 else "Operação") + ")"
        
    class Meta:
        ordering = ['nome']

class Contas(models.Model):
    id = models.AutoField(primary_key=True)
    nome = models.CharField(max_length=200, blank=True, null=True)
    status = models.IntegerField( blank=True, null=True)
    cat_id = models.ForeignKey(Categorias, on_delete=models.CASCADE, blank=True, null=True, verbose_name="categorias")
    conta = models.ForeignKey('self', on_delete=models.CASCADE, blank=True, null=True, verbose_name="contas")
    dt_criacao = models.DateTimeField(default=timezone.now)
    dt_alteracao = models.DateTimeField(blank=True, null=True)
    
    def publish(self):
        self.dt_alteracao = timezone.now()
        self.save()
        
    def __str__(self):
        return self.nome
        
    class Meta:
        ordering = ['nome']
        
        
class Operacoes(models.Model):
    id = models.AutoField(primary_key=True)
    nome = models.CharField(max_length=200, blank=True, null=True)
    tipo = models.IntegerField(choices=TipoOperacao.choices)
    recorrente = models.IntegerField(default=0, choices=SimNao.choices)
    salario = models.IntegerField(default=0, choices=SimNao.choices)
    status = models.IntegerField(default=1, choices=SimNao.choices)
    cat_id = models.ForeignKey(Categorias, on_delete=models.CASCADE, blank=True, null=True, verbose_name="categorias")
    dt_criacao = models.DateTimeField(default=timezone.now)
    dt_alteracao = models.DateTimeField(blank=True, null=True)
    
    def publish(self):
        self.dt_alteracao = timezone.now()
        self.save()

    def __str__(self):
        return self.nome + " (" + ("Crédito" if self.tipo == 1 else "Débito") + ")"
        
    class Meta:
        ordering = ['nome']

class ContasOperacoes(models.Model):
    id = models.AutoField(primary_key=True)
    conta = models.ForeignKey(Contas, on_delete=models.CASCADE, blank=True, null=True, verbose_name="contas")
    operacao = models.ForeignKey(Operacoes, on_delete=models.CASCADE, blank=True, null=True, verbose_name="operacoes")
    tipo = models.IntegerField( blank=True, null=True)
    dt_base = models.DateTimeField(blank=True, null=True)
    dt_criacao = models.DateTimeField(default=timezone.now)
    dt_alteracao = models.DateTimeField(blank=True, null=True)
    
    def publish(self):
        self.dt_alteracao = timezone.now()
        self.save()
    
class Movimentos(models.Model):
    id = models.AutoField(primary_key=True)
    valor = models.FloatField(blank=True, null=True)
    observacao = models.CharField(max_length=200, blank=True, null=True)
    consolidado = models.IntegerField(blank=True, null=True, choices=SimNao.choices)
    parcela = models.IntegerField(blank=True, null=True)
    total_parcelas = models.IntegerField(blank=True, null=True)
    movimento_pai = models.ForeignKey('self', on_delete=models.CASCADE, blank=True, null=True, verbose_name="Movimento")
    cta_id = models.ForeignKey(Contas, on_delete=models.CASCADE, blank=True, null=True, verbose_name="Conta")
    conta_destino = models.ForeignKey(Contas, on_delete=models.CASCADE, blank=True, null=True, verbose_name="Conta Destino", related_name="conta_destino")
    ope_id = models.ForeignKey(Operacoes, on_delete=models.CASCADE, blank=True, null=True, verbose_name="Operação")
    dt_criacao = models.DateTimeField(default=timezone.now)
    dt_alteracao = models.DateTimeField(blank=True, null=True, auto_now=True)
    
    def valor_convertido(self):
        return self.valor * -1 if self.ope_id.tipo == 2 else self.valor
        
    def publish(self):
        self.dt_alteracao = timezone.now()
        self.save()
                
    class Meta:
        ordering = ['ope_id__tipo', 'dt_criacao']
        indexes = [
            models.Index(fields=['dt_criacao',]),
        ]
            
class Saldos(models.Model):
    id = models.AutoField(primary_key=True)
    valor = models.FloatField(blank=True, null=True)
    vlr_acumulado = models.FloatField(blank=True, null=True)
    vlr_valorizacao = models.FloatField(blank=True, null=True)
    vlr_dividendo = models.FloatField(blank=True, null=True)
    vlr_rendimento = models.FloatField(blank=True, null=True)
    perc_valorizacao = models.FloatField(blank=True, null=True)
    perc_rendimento = models.FloatField(blank=True, null=True)
    credito = models.FloatField(blank=True, null=True)
    debito = models.FloatField(blank=True, null=True)
    credito_salario = models.FloatField(blank=True, null=True)
    debito_salario = models.FloatField(blank=True, null=True)
    entrada = models.FloatField(blank=True, null=True)
    saida = models.FloatField(blank=True, null=True)
    cta_id = models.ForeignKey(Contas, on_delete=models.CASCADE, blank=True, null=True, verbose_name="contas")
    dt_saldo = models.DateTimeField(blank=True, null=True)
    dt_criacao = models.DateTimeField(default=timezone.now)
    dt_alteracao = models.DateTimeField(blank=True, null=True)
    
    def publish(self):
        self.dt_alteracao = timezone.now()
        self.save()
        
    def __str__(self):
        return self.cta_id.nome + "(" + self.dt_saldo.strftime('%d/%m/%Y') + ")"
        
class SaldoProxy(Saldos):

    class Meta:
        proxy = True
        verbose_name_plural = "Dashboard"
        
class SaldoSimulado(models.Model):
    id = models.AutoField(primary_key=True)
    vlr_real = models.FloatField(blank=True, null=True)
    vlr_diferenca = models.FloatField(blank=True, null=True)
    saldo = models.ForeignKey(Saldos, on_delete=models.CASCADE, blank=True, null=True, verbose_name="saldos")
    ajusta = models.IntegerField(default=0)
    dt_criacao = models.DateTimeField(default=timezone.now)
    dt_alteracao = models.DateTimeField(blank=True, null=True)
    
    def save(self, *args, **kwargs):
        self.dt_alteracao = timezone.now()
        self.vlr_diferenca = round(self.vlr_real - self.saldo.vlr_acumulado, 2)
        super(SaldoSimulado, self).save(*args, **kwargs)
        
    class Meta:
        ordering = ['saldo__dt_saldo']

class Execucoes(models.Model):
    id = models.AutoField(primary_key=True)
    executa_saldo = models.IntegerField(default=0)
    dt_exec_saldo = models.DateField(blank=True, null=True)
    executa_recorrencia = models.IntegerField(default=0)
    dt_exec_rec = models.DateField(blank=True, null=True)
    executa_saldo_sim = models.IntegerField(default=0)
    dt_exec_saldo_sim = models.DateField(blank=True, null=True)
    dt_criacao = models.DateTimeField(default=timezone.now)
    dt_alteracao = models.DateTimeField(blank=True, null=True)
    
    def publish(self):
        self.dt_alteracao = timezone.now()
        self.save()

class TiposAtivos(models.Model):
    id = models.AutoField(primary_key=True)
    codigo = models.CharField(max_length=5, blank=True, null=True)
    descricao = models.CharField(max_length=50, blank=True, null=True)
    conta = models.ForeignKey(Contas, on_delete=models.CASCADE, blank=True, null=True, verbose_name="contas")
    dt_criacao = models.DateTimeField(default=timezone.now)
    dt_alteracao = models.DateTimeField(blank=True, null=True)
    
    def publish(self):
        self.dt_alteracao = timezone.now()
        self.save()
        
    def __str__(self):
        return self.codigo
        
class Ativos(models.Model):
    id = models.AutoField(primary_key=True)
    nome = models.CharField(max_length=200, blank=True, null=True) 
    vlr_compra = models.FloatField(blank=True, null=True)
    vlr_venda = models.FloatField(blank=True, null=True)
    perc_irpf = models.FloatField(blank=True, null=True)
    vlr_irpf = models.FloatField(blank=True, null=True)
    vlr_despesas = models.FloatField(default=0.0)
    vlr_retorno_bruto = models.FloatField(blank=True, null=True)
    vlr_retorno_liq = models.FloatField(blank=True, null=True)
    perc_retorno_liq = models.FloatField(default=0.0)
    periodo_meses = models.IntegerField(blank=True, null=True)
    dt_vencimento = models.DateTimeField(blank=True, null=True)
    situacao = models.IntegerField(default=0)
    tipo_ativo = models.ForeignKey(TiposAtivos, on_delete=models.CASCADE, blank=True, null=True, verbose_name="tipos_ativos")
    dt_criacao = models.DateTimeField(default=timezone.now)
    dt_alteracao = models.DateTimeField(blank=True, null=True)
    
    def save(self, *args, **kwargs):
        self.dt_alteracao = timezone.now()
        self.vlr_retorno_bruto = round(self.vlr_venda - self.vlr_compra, 2) if self.vlr_venda != None else 0.0
        self.periodo_meses = (self.dt_vencimento.year - self.dt_criacao.year) * 12 + (self.dt_vencimento.month - self.dt_criacao.month) + round((self.dt_vencimento.day - self.dt_criacao.day) / 30) if self.dt_vencimento != None else 1
        self.periodo_meses = self.periodo_meses if self.periodo_meses > 0 else 1
        self.perc_irpf = (22.50 if self.periodo_meses < 6 else 20.00 if self.periodo_meses >= 6 and self.periodo_meses < 12 else 17.50) if self.tipo_ativo.codigo == 'CDB' else 0.0
        self.vlr_irpf = round(self.vlr_retorno_bruto * (self.perc_irpf / 100), 2)
        self.vlr_retorno_liq = round(self.vlr_retorno_bruto - self.vlr_irpf - self.vlr_despesas, 2)
        self.perc_retorno_liq = round((self.vlr_retorno_liq * 100) / self.vlr_compra / self.periodo_meses * 12, 2)
        super(Ativos, self).save(*args, **kwargs)

    class Meta:
        ordering = ['dt_vencimento']
        