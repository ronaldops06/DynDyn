# Generated by Django 3.2.5 on 2021-08-02 17:36

from django.db import migrations, models
import django.utils.timezone


class Migration(migrations.Migration):

    dependencies = [
        ('website', '0004_auto_20210802_1327'),
    ]

    operations = [
        migrations.CreateModel(
            name='Execucoes',
            fields=[
                ('id', models.AutoField(primary_key=True, serialize=False)),
                ('executa_saldo', models.IntegerField(default=0)),
                ('dt_exec_saldo', models.DateField(blank=True, null=True)),
                ('dt_criacao', models.DateTimeField(default=django.utils.timezone.now)),
                ('dt_alteracao', models.DateTimeField(blank=True, null=True)),
            ],
        ),
    ]