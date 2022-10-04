# Assalomu alaykum

### Demak bu proyekt o'zi nima qiladi va qanday ishlatiladi hozir shu haqida aytib o'taman

**Birinchi ConnectionString ni to'g'irlang appsettings.json faylini ichidan**

## dotnet 7.0 ni https://dotnet.microsoft.com/en-us/download/dotnet/thank-you/sdk-7.0.100-rc.1-windows-x86-installer shu yerdan olamiz.

## Keyin man buni bazasi sifatida Sqlite ishlatgandim uni https://download.sqlitebrowser.org/DB.Browser.for.SQLite-3.12.2-win64.msi shundan olamiz.
## Birinchi

> #### Proyektni ishlatish haqida
>
> - **Birinchi** NAPA.WebApp faylini ichidagi Running.bat ishlatiladi
>
>  *QQS* ni  **appsettings.json** dagi VAT dan o'zgartiriladi.(Bu qo'simcha qiymat so'lig'i)

Ikkita rol bor
- Admin **Administrator**
- User **Foydalanuvchi**

*Administrator*
- Email **shaxnoza@mail.ru**
- Parol **Sh@xn0za2005**

*Administrator*
- Email **testadmin@mail.ru**
- Parol **@dM1n2006**

*Foydalanuvchi*
- Email **ruzimurodabdunazarov2003@mail.ru**
- Parol **R@zik2003**

**Administrator** hamma rollarni ham boshqara oladi.
Yaniki kimnidir rolini bemalol o'zgartira oladi.

*Log* larni bazada **product_audit** dan topsa ham bo'ladi.(buni **sqlite** ni yuklagandan so'ng ochsa bo'ladi)

Yoki umumiy *migratsiya*larni **console** ga yozib ketadi(bazi bir sabablarga ko'ra).
O'zi qanaqadir faylni ichiga yozsa bo'ladi,lekin bir vaqtni o'zida ikki protses bajarila olmaydi.

Faqat *Administrator* CRUD qila oladi.(Har bir o'zgarish bazaga va console ga yozib boriladi)

Rollarni boshqarish uchun yuqorida **Roles** degan knopka bor.

Ha aytgancha qanaqadir boshqa rolli odam tizimga kiraman desa tizim unga **Siz bunaqa rolga ega emassiz** deydi.

Manabu ikki foydalanuvchi test uchun.

Agar *kimdir* boshida ro'yhatdan o'tayotgan bo'lsa uni rolini har doimo User qilib saqlab qo'yadi.

Keyin *administrator* kirib o'zgartirsa bo'ladi.

Test uchun o'ziz boshqa foydalanuvhci bo'lib kirib,
Administrator orqali rolini oshirib uni ham administrator qila olasiz.

Agar /user zaprosi bilan fromDate,toDate query lari ketsa bu shu vaqtlar orasida ro'yhatdan o'tgan userlarni *Json* shaklida qaytaradi.(Faqat administratorlar uchun)

agar /product_audit zaprosi ketsa umumiy bazadagi o'zgarishlar Json formatida qaytib keladi.(Faqat administratorlar uchun)


Proyektni **Vat** ni hisoblayotganini tekshirish uchun bitta *UnitTest* proyekt bor.

> #### U yerda ikki hil tekshiradi
>
> - **Birinchi** Bizning bazadagi **TotalPrice** bilan test holatdagi **TotalPrice** ni
> - **Ikkinchi** Yangi bitta *obyekt* tuzib o'sha obyektni **TotalPrice** qiymati bo'yicha
>  

Faqat connectionString ga etibor berib to'g'rlab qo'yish kerak.
Hozir *d:\\Ruzimurod\\for-github\\napa-automotive\\NAPA.db* shunaqa turibdi.

Lekin siz proyektni yuklasez o'sha yer joylashgan **Path** ni kiritasiz

Layoutga Web Api , Roles , About users , Product audits , Log out lar bor