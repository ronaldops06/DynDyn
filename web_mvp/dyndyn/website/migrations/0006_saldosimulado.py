# Generated by Django 3.2.5 on 2021-08-02 19:02

from django.db import migrations, models
import django.db.models.deletion
import django.utils.timezone


class Migration(migrations.Migration):

    dependencies = [
        ('website', '0005_execucoes'),
    ]

    operations = [
        migrations.CreateModel(
            name='SaldoSimulado',
            fields=[
                ('id', models.AutoField(primary_key=True, serialize=False)),
                ('vlr_real', models.FloatField(blank=True, null=True)),
                ('vlr_diferenca', models.FloatField(blank=True, null=True)),
                ('dt_criacao', models.DateTimeField(default=django.utils.timezone.now)),
                ('dt_alteracao', models.DateTimeField(blank=True, null=True)),
                ('saldo', models.ForeignKey(blank=True, null=True, on_delete=django.db.models.deletion.CASCADE, to='website.saldos', verbose_name='saldos')),
            ],
            options={
                'ordering': ['saldo__dt_saldo'],
            },
        ),
    ]
