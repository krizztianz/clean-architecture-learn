# Accounting Journal

Project pembelajaran **Clean Architecture + CQRS + Custom Mediator Pattern** menggunakan **.NET 8**.

Project ini dibuat sebagai latihan bertahap untuk memahami bagaimana aplikasi backend dibangun dengan pemisahan tanggung jawab yang rapi antara **Domain**, **Application**, **Infrastructure**, **API**, dan **Tests**.

> Fokus utama project ini bukan hanya membuat fitur berjalan, tetapi memahami **kenapa setiap layer ada**, **apa tanggung jawabnya**, **batas akses antar-layer**, dan **apa dampaknya jika aturan arsitektur dilanggar**.

---

## 1. Tujuan Project

Project ini mensimulasikan aplikasi backend sederhana untuk pencatatan jurnal akuntansi.

Secara bisnis, aplikasi ini mengelola **journal entry** yang berisi baris debit dan kredit. Sebuah jurnal harus seimbang sebelum bisa diposting.

Secara teknis, project ini digunakan untuk mempelajari:

- Clean Architecture
- Domain-Driven Design dasar
- Aggregate Root
- Entity dan Value Object
- Domain validation
- CQRS: Command dan Query
- Custom Mediator Pattern tanpa MediatR
- Dependency Injection
- Repository Pattern
- Unit of Work
- Unit Testing untuk Domain dan Application layer

---

## 2. Struktur Solution

```txt
AccountingJournal.sln
src/
  AccountingJournal.Api/
  AccountingJournal.Application/
  AccountingJournal.Domain/
  AccountingJournal.Infrastructure/
tests/
  AccountingJournal.Application.Tests/
  AccountingJournal.Domain.Tests/
