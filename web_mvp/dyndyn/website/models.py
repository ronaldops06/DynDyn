from django.db import models
from django.utils import timezone

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
    tipo = models.IntegerField( blank=True, null=True)
    recorrente = models.IntegerField(default=0)
    status = models.IntegerField( blank=True, null=True)
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
    
class Movimentos(models.Model):
    id = models.AutoField(primary_key=True)
    valor = models.FloatField(blank=True, null=True)
    observacao = models.CharField(max_length=200, blank=True, null=True)
    cta_id = models.ForeignKey(Contas, on_delete=models.CASCADE, blank=True, null=True, verbose_name="contas")
    ope_id = models.ForeignKey(Operacoes, on_delete=models.CASCADE, blank=True, null=True, verbose_name="operacoes")
    dt_criacao = models.DateTimeField(default=timezone.now)
    dt_alteracao = models.DateTimeField(blank=True, null=True)
        
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
    credito = models.FloatField(blank=True, null=True)
    debito = models.FloatField(blank=True, null=True)
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
        
class SaldoSimulado(models.Model):
    id = models.AutoField(primary_key=True)
    vlr_real = models.FloatField(blank=True, null=True)
    vlr_diferenca = models.FloatField(blank=True, null=True)
    saldo = models.ForeignKey(Saldos, on_delete=models.CASCADE, blank=True, null=True, verbose_name="saldos")
    dt_criacao = models.DateTimeField(default=timezone.now)
    dt_alteracao = models.DateTimeField(blank=True, null=True)
    
    def publish(self):
        self.dt_alteracao = timezone.now()
        self.save()
        
    class Meta:
        ordering = ['saldo__dt_saldo']

class Execucoes(models.Model):
    id = models.AutoField(primary_key=True)
    executa_saldo = models.IntegerField(default=0)
    dt_exec_saldo = models.DateField(blank=True, null=True)
    executa_recorrencia = models.IntegerField(default=0)
    dt_exec_rec = models.DateField(blank=True, null=True)
    dt_criacao = models.DateTimeField(default=timezone.now)
    dt_alteracao = models.DateTimeField(blank=True, null=True)
    
    def publish(self):
        self.dt_alteracao = timezone.now()
        self.save()