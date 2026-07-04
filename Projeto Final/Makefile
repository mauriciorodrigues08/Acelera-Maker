# ================================================================
# Makefile — Projeto Final COBOL / Modernização Cooperativa Alfa
# ================================================================
# Alvos disponíveis:
#   make          → compila tudo (padrão)
#   make clean    → remove artefatos de build
#   make db-init  → cria banco e popula com dados de exemplo
#   make run-consulta  → testa consulta de cliente (codigo 1)
#   make run-atualiza  → testa atualização de cliente (codigo 1)
#   make run-nao-encontrado → testa cliente inexistente (codigo 99)
#   make test     → roda todos os testes acima em sequência
# ================================================================

# ── Compiladores e flags ────────────────────────────────────────
CC      = gcc
COBC    = cobc
CFLAGS  = -Wall
COBCFLAGS = -x -free
LIBS    = -lodbc

# ── Diretorios ─────────────────────────────────────────────────
COBOL_DIR   = cobol
BUILD_DIR = build
DATA_DIR  = data

# ── Arquivos fonte ──────────────────────────────────────────────
C_SRC     = $(COBOL_DIR)/sqlitebridge.c
COB_SRC   = $(COBOL_DIR)/clientes.cob

# ── Artefatos de build ──────────────────────────────────────────
C_OBJ     = $(BUILD_DIR)/sqlitebridge.o
EXECUTABLE = $(BUILD_DIR)/clientes

# ── Banco de dados ──────────────────────────────────────────────
DB_FILE   = $(DATA_DIR)/clientes.db

# ── ODBC ────────────────────────────────────────────────────────
export ODBCINI = $(CURDIR)/odbc.ini

# ================================================================
# ALVO PADRÃO
# ================================================================
.PHONY: all
all: $(BUILD_DIR) $(EXECUTABLE)
	@echo ""
	@echo "✔  Build concluído: $(EXECUTABLE)"
	@echo "   Para testar: make test"

# ── Garante que o diretório build existe ─────────────────────────
$(BUILD_DIR):
	mkdir -p $(BUILD_DIR)

# ── Compila o wrapper C (sqlitebridge) ───────────────────────────
$(C_OBJ): $(C_SRC) | $(BUILD_DIR)
	$(CC) $(CFLAGS) -c $(C_SRC) -o $(C_OBJ)

# ── Compila o programa COBOL principal ───────────────────────────
$(EXECUTABLE): $(COB_SRC) $(C_OBJ)
	$(COBC) $(COBCFLAGS) $(COB_SRC) $(C_OBJ) -o $(EXECUTABLE) $(LIBS)

# ================================================================
# BANCO DE DADOS
# ================================================================
.PHONY: db-init
db-init:
	@echo "Inicializando banco de dados em $(DB_FILE)..."
	@mkdir -p $(DATA_DIR)
	@sqlite3 $(DB_FILE) " \
		CREATE TABLE IF NOT EXISTS clientes ( \
			codigo    INTEGER PRIMARY KEY, \
			nome      TEXT    NOT NULL, \
			telefone  TEXT    NOT NULL, \
			email     TEXT    NOT NULL \
		); \
		INSERT OR IGNORE INTO clientes VALUES (1,'Joao Silva','11999999999','joao@teste.com'); \
		INSERT OR IGNORE INTO clientes VALUES (2,'Maria Souza','11988888888','maria@teste.com'); \
		INSERT OR IGNORE INTO clientes VALUES (3,'Carlos Lima','11977777777','carlos@teste.com'); \
	"
	@echo "✔  Banco inicializado com 3 clientes de exemplo."

# ================================================================
# TESTES
# ================================================================
.PHONY: run-consulta
run-consulta: all
	@echo ""
	@echo "── Consulta: cliente existente (codigo 1) ──────────────"
	@echo '{"operacao":"C","codigo":1}' | $(EXECUTABLE)

.PHONY: run-atualiza
run-atualiza: all
	@echo ""
	@echo "── Atualização: telefone e email do cliente 1 ──────────"
	@echo '{"operacao":"A","codigo":1,"telefone":"11955554444","email":"joao.atualizado@teste.com"}' | $(EXECUTABLE)
	@echo ""
	@echo "── Consulta após atualização ───────────────────────────"
	@echo '{"operacao":"C","codigo":1}' | $(EXECUTABLE)

.PHONY: run-nao-encontrado
run-nao-encontrado: all
	@echo ""
	@echo "── Consulta: cliente inexistente (codigo 99) ───────────"
	@echo '{"operacao":"C","codigo":99}' | $(EXECUTABLE)

.PHONY: test
test: run-consulta run-atualiza run-nao-encontrado
	@echo ""
	@echo "✔  Todos os testes concluídos."

# ================================================================
# LIMPEZA
# ================================================================
.PHONY: clean
clean:
	@echo "Removendo artefatos de build..."
	rm -rf $(BUILD_DIR)
	@echo "✔  Limpeza concluída."