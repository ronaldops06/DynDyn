# Generated by Django 3.2.5 on 2022-05-07 22:40

from django.db import migrations, models


class Migration(migrations.Migration):

    dependencies = [
        ('website', '0015_contasoperacoes_dt_base'),
    ]

    operations = [
        migrations.AddField(
            model_name='saldosimulado',
            name='executa',
            field=models.IntegerField(default=0),
        ),
    ]
