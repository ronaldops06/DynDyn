# Generated by Django 3.2.5 on 2023-03-22 13:46

from django.db import migrations, models


class Migration(migrations.Migration):

    dependencies = [
        ('website', '0030_auto_20230322_1247'),
    ]

    operations = [
        migrations.AlterField(
            model_name='operacoes',
            name='tipo',
            field=models.IntegerField(blank=True, choices=[(1, 'Crédito'), (2, 'Débito'), (3, 'Transferência')], null=True),
        ),
    ]
