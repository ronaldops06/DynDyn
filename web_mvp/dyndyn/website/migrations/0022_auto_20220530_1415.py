# Generated by Django 3.2.5 on 2022-05-30 17:15

from django.db import migrations, models


class Migration(migrations.Migration):

    dependencies = [
        ('website', '0021_ativos_saldoproxy'),
    ]

    operations = [
        migrations.AlterModelOptions(
            name='ativos',
            options={'ordering': ['dt_vencimento']},
        ),
        migrations.AlterField(
            model_name='ativos',
            name='vlr_despesas',
            field=models.FloatField(default=0.0),
        ),
    ]
