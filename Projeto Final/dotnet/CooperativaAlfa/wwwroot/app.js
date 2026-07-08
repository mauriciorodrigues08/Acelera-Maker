const API = '';
let codigoAtual = null;

/* ── Utilitários de UI ─────────────────────────────────────── */
function mostrarAlerta(id, tipo, msg) {
  const el = document.getElementById(id);
  el.className = `alert alert-${tipo} visible`;
  el.textContent = msg;
}

function esconderAlerta(id) {
  const el = document.getElementById(id);
  el.className = 'alert';
  el.textContent = '';
}

function mostrar(id)  { document.getElementById(id).classList.add('visible'); }
function esconder(id) { document.getElementById(id).classList.remove('visible'); }

function setLoading(btnId, ativo, textoOriginal, textoLoading) {
  const btn = document.getElementById(btnId);
  btn.disabled = ativo;
  btn.innerHTML = ativo
    ? `<span class="spinner"></span>${textoLoading}`
    : textoOriginal;
}

/* ── Buscar cliente ────────────────────────────────────────── */
async function buscar() {
  const codigo = parseInt(document.getElementById('inputCodigo').value);

  esconderAlerta('alertaBusca');
  esconder('cardCliente');
  esconder('formEdicao');

  if (!codigo || codigo <= 0) {
    mostrarAlerta('alertaBusca', 'error',
      'Informe um código válido (número inteiro positivo).');
    return;
  }

  setLoading('btnBuscar', true, 'Buscar', 'Buscando...');

  try {
    const res  = await fetch(`${API}/clientes/${codigo}`);
    const data = await res.json();

    if (res.status === 200) {
      codigoAtual = data.codigo;

      document.getElementById('avatarLetra').textContent =
        (data.nome || '?')[0].toUpperCase();
      document.getElementById('clienteNome').textContent     = data.nome;
      document.getElementById('clienteCodigo').textContent   = `Código: ${data.codigo}`;
      document.getElementById('clienteTelefone').textContent = data.telefone;
      document.getElementById('clienteEmail').textContent    = data.email;

      mostrar('cardCliente');

    } else if (res.status === 404) {
      mostrarAlerta('alertaBusca', 'info',
        `Cliente com código ${codigo} não encontrado.`);
    } else {
      mostrarAlerta('alertaBusca', 'error',
        data.mensagem || 'Erro interno. Tente novamente.');
    }
  } catch (e) {
    mostrarAlerta('alertaBusca', 'error',
      'Não foi possível conectar à API. Verifique se o servidor está rodando.');
  } finally {
    setLoading('btnBuscar', false, 'Buscar', 'Buscando...');
  }
}

/* ── Formulário de edição ──────────────────────────────────── */
function abrirEdicao() {
  document.getElementById('inputTelefone').value =
    document.getElementById('clienteTelefone').textContent;
  document.getElementById('inputEmail').value =
    document.getElementById('clienteEmail').textContent;

  esconderAlerta('alertaEdicao');
  mostrar('formEdicao');
  document.getElementById('inputTelefone').focus();
}

function cancelarEdicao() {
  esconder('formEdicao');
  esconderAlerta('alertaEdicao');
}

async function salvar() {
  const telefone = document.getElementById('inputTelefone').value.trim();
  const email    = document.getElementById('inputEmail').value.trim();

  esconderAlerta('alertaEdicao');

  if (!/^\d{10,15}$/.test(telefone)) {
    mostrarAlerta('alertaEdicao', 'error',
      'Telefone inválido. Use somente dígitos (10 a 15 caracteres).');
    return;
  }

  if (!email.includes('@') || email.length > 60) {
    mostrarAlerta('alertaEdicao', 'error', 'E-mail inválido.');
    return;
  }

  setLoading('btnSalvar', true, '💾 Salvar', 'Salvando...');

  try {
    const res  = await fetch(`${API}/clientes/${codigoAtual}`, {
      method:  'PUT',
      headers: { 'Content-Type': 'application/json' },
      body:    JSON.stringify({ telefone, email })
    });
    const data = await res.json();

    if (res.status === 200) {
      document.getElementById('clienteTelefone').textContent = telefone;
      document.getElementById('clienteEmail').textContent    = email;
      esconder('formEdicao');
      mostrarAlerta('alertaBusca', 'success', 'Dados atualizados com sucesso!');

    } else if (res.status === 404) {
      mostrarAlerta('alertaEdicao', 'error', 'Cliente não encontrado.');
    } else {
      mostrarAlerta('alertaEdicao', 'error',
        data.mensagem || 'Erro ao salvar. Tente novamente.');
    }
  } catch (e) {
    mostrarAlerta('alertaEdicao', 'error',
      'Não foi possível conectar à API.');
  } finally {
    setLoading('btnSalvar', false, '💾 Salvar', 'Salvando...');
  }
}

/* ── Event listeners ───────────────────────────────────────── */
document.addEventListener('DOMContentLoaded', () => {
  document.getElementById('inputCodigo')
    .addEventListener('keydown', e => {
      if (e.key === 'Enter') buscar();
    });
});