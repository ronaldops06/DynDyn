#!/bin/bash

set -e

PROJECT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"

echo "=========================================="
echo " SAGE MONEY - INSTALAÇÃO LIMPA"
echo "=========================================="
echo
echo "Projeto: $PROJECT_DIR"
echo

cd "$PROJECT_DIR"

echo "[1/9] Parando processos do Gradle..."
cd android
./gradlew --stop || true
cd ..

echo
echo "[2/9] Removendo node_modules..."
rm -rf node_modules

echo
echo "[3/9] Removendo caches do Metro/Babel..."
rm -rf node_modules/.cache
rm -rf /tmp/metro-*
rm -rf /tmp/haste-map-*

echo
echo "[4/9] Removendo arquivos de build Android..."
rm -rf android/.gradle
rm -rf android/build
rm -rf android/app/build

echo
echo "[5/9] Limpando build do Gradle..."
cd android
./gradlew clean
cd ..

echo
echo "[6/9] Limpando cache do npm..."
npm cache verify

echo
echo "[7/9] Instalando dependências..."
npm install

echo
echo "[8/9] Verificando versões principais..."
echo
echo "React Native:"
npm ls react-native --depth=0 || true

echo
echo "React:"
npm ls react --depth=0 || true

echo
echo "Gesture Handler:"
npm ls react-native-gesture-handler --depth=0 || true

echo
echo "[9/9] Limpeza concluída."
echo
echo "=========================================="
echo " PRÓXIMOS PASSOS"
echo "=========================================="
echo
echo "1. Inicie o Metro:"
echo "   npx react-native start --reset-cache"
echo
echo "2. Em outro terminal:"
echo "   npx react-native run-android"
echo
