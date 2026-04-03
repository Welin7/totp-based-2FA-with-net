# 🔐 OTP Auth API

Uma **Minimal API** em C# (.NET) para geração e validação de senhas de uso único baseadas em tempo (**TOTP**), compatível com autenticadores como Google Authenticator e Authy.

---

## 📋 Visão Geral

Este projeto implementa autenticação de dois fatores (2FA) via TOTP (Time-based One-Time Password) seguindo o padrão [RFC 6238](https://datatracker.ietf.org/doc/html/rfc6238). A API expõe dois endpoints: um para geração de QR Code e outro para validação do código OTP.

---

## 🚀 Endpoints

### `GET /otp/qrcode`

Gera um QR Code em formato PNG contendo a URI `otpauth://` para configuração do autenticador.

**Resposta:**
- `200 OK` — Imagem PNG do QR Code
- Content-Type: `image/png`

**Exemplo de URI gerada:**
```
otpauth://totp/MyAppOtpAuth:user_test%40gmail.com?secret=BASE32SECRET&issuer=MyAppOtpAuth&digits=6&period=30
```

---

### `POST /otp/validate`

Valida um código TOTP de 6 dígitos gerado pelo autenticador do usuário.

**Body (JSON):**
```json
{
  "code": "123456"
}
```

**Respostas:**
| Status | Mensagem |
|--------|----------|
| `200 OK` | `"Código OTP válido"` |
| `400 Bad Request` | `"Código OTP inválido"` |

---

## 🛠️ Tecnologias

| Pacote | Finalidade |
|--------|-----------|
| [Otp.NET](https://github.com/kspearrin/Otp.NET) | Geração e verificação de tokens TOTP |
| [QRCoder](https://github.com/codebude/QRCoder) | Geração de QR Codes em PNG |
| ASP.NET Core Minimal API | Framework web leve e performático |

---

## ⚙️ Como Executar

### Pré-requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download) ou superior

### Passos

```bash
# Clone o repositório
git clone https://github.com/seu-usuario/otp-auth-api.git
cd otp-auth-api

# Restaure as dependências
dotnet restore

# Execute a aplicação
dotnet run
```

A API estará disponível em `https://localhost:5001` ou `http://localhost:5000`.

---

## 📱 Testando com Autenticador

1. Acesse `GET /otp/qrcode` no navegador — o QR Code será exibido automaticamente.
2. Abra o **Google Authenticator**, **Authy** ou qualquer app compatível com TOTP.
3. Escaneie o QR Code.
4. Use o código de 6 dígitos gerado pelo app para chamar `POST /otp/validate`.

---

## 🔒 Segurança

- A **chave secreta** é gerada aleatoriamente a cada inicialização da aplicação (`KeyGeneration.GenerateRandomKey()`).
- Em produção, a chave secreta deve ser **armazenada de forma segura e persistida** (ex: banco de dados com criptografia em repouso), associada a cada usuário individualmente.
- A validação utiliza `VerificationWindow.RfcSpecifiedNetworkDelay` para tolerar pequenas diferenças de sincronização de relógio entre cliente e servidor.
- O padrão TOTP usa janelas de **30 segundos** com códigos de **6 dígitos**.

> ⚠️ **Atenção:** A implementação atual usa uma chave em memória (`byte[] secretKey`) e um usuário fixo (`user_test@gmail.com`). Isso é adequado apenas para fins de demonstração. Para uso em produção, implemente gerenciamento de usuários e persistência segura da chave.

---

## 📁 Estrutura do Projeto

```
otp-auth-api/
├── Dtos/
│   └── ValidateOtpRequestDto.cs   # DTO para o body do endpoint de validação
├── Program.cs                     # Configuração da aplicação e definição dos endpoints
└── otp-auth-api.csproj            # Definição do projeto e dependências NuGet
```

---

## 📦 Dependências NuGet

```xml
<PackageReference Include="Otp.NET" Version="*" />
<PackageReference Include="QRCoder" Version="*" />
```

---

## 📄 Licença

Este projeto está licenciado sob a [MIT License](LICENSE).

---

## 🤝 Contribuições

Contribuições são bem-vindas! Abra uma *issue* ou envie um *pull request*.

1. Faça um fork do projeto
2. Crie uma branch para sua feature (`git checkout -b feature/minha-feature`)
3. Commit suas mudanças (`git commit -m 'feat: adiciona minha feature'`)
4. Push para a branch (`git push origin feature/minha-feature`)
5. Abra um Pull Request
