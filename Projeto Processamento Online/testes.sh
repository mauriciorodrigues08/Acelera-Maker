#!/bin/bash
# ---------------------------------------------------------------------
# testes.sh - Bateria de testes do projeto CLIPGM (Consulta de Clientes)
#
# Como usar:
#   1. Compile antes:  cobc -x src/CARGA.cbl  &&  cobc -x src/CLIPGM.cbl
#   2. Rode este script a partir da RAIZ do projeto:  bash testes.sh
#
# O script reseta a massa de dados antes de cada rodada, para os
# testes não interferirem uns nos outros.
# ---------------------------------------------------------------------

echo "============================================="
echo " Resetando massa de dados (./CARGA)"
echo "============================================="
rm -f data/clientes.dat
./CARGA
echo ""

echo "============================================="
echo " TESTE 1 - Consultar cliente existente (000005)"
echo " Esperado: CLIENTE ENCONTRADO + dados do Pedro Henrique"
echo "============================================="
printf "5\n000005\n3\n" | ./CLIPGM | grep -E "Nome|Mensagem"
echo ""

echo "============================================="
echo " TESTE 2 - Consultar codigo inexistente (000050)"
echo " Esperado: CLIENTE NAO ENCONTRADO"
echo "============================================="
printf "5\n000050\n3\n" | ./CLIPGM | grep "Mensagem"
echo ""

echo "============================================="
echo " TESTE 3 - Salvar SEM consultar antes (direto opcao 6)"
echo " Esperado: CONSULTE ANTES DE SALVAR"
echo "============================================="
printf "6\n3\n" | ./CLIPGM | grep "Mensagem"
echo ""

echo "============================================="
echo " TESTE 4 - Consultar -> Salvar -> Sair -> persistencia"
echo " Esperado: ALTERACAO REALIZADA + dados batendo no arquivo"
echo "============================================="
printf "5\n000002\n6\n(35)90001-2345\nMACHADO\n3\n" | ./CLIPGM | grep "Mensagem"
echo "Conferindo no arquivo:"
grep 000002 data/clientes.dat
echo ""

echo "============================================="
echo " TESTE 5 - Opcao invalida (9)"
echo " Esperado: OPCAO INVALIDA"
echo "============================================="
printf "9\n3\n" | ./CLIPGM | grep "Mensagem"
echo ""

echo "============================================="
echo " TESTE 6 - Codigo em branco no consultar"
echo " Esperado: CODIGO OBRIGATORIO"
echo "============================================="
printf "5\n\n3\n" | ./CLIPGM | grep "Mensagem"
echo ""

echo "============================================="
echo " TESTE 7 - Trocar de cliente sem salvar o anterior"
echo " Esperado: so o ULTIMO consultado (000003) deve mudar"
echo "============================================="
printf "5\n000001\n5\n000003\n6\nNOVO TEL\nBH ALTERADA\n3\n" | ./CLIPGM > /dev/null
echo "Conferindo no arquivo (000001 intacto, 000003 alterado):"
grep -E "^00000(1|3)" data/clientes.dat
echo ""

echo "============================================="
echo " TESTE 8 - Enter em branco no PF6 (nao deve apagar dados)"
echo " Esperado: ALTERACAO REALIZADA, mas dados originais mantidos"
echo "============================================="
printf "5\n000004\n6\n\n\n3\n" | ./CLIPGM | grep "Mensagem"
echo "Conferindo no arquivo (deve continuar igual ao original):"
grep 000004 data/clientes.dat
echo ""

echo "============================================="
echo " FIM DOS TESTES"
echo "============================================="