# ShortCuts
Все действия выполняются в CLI
## Установка dotnet sdk
![Microsoft](http://qrcoder.ru/code/?https%3A%2F%2Fdocs.microsoft.com%2Fru-ru%2Fdotnet%2Fcore%2Finstall%2F&6&0)

## Установка Scaffold
[Microsoft](https://docs.microsoft.com/ru-ru/ef/core/get-started/overview/install)

```bash
dotnet tool install --global dotnet-ef
```
## Добавление пакетов в проект
```bash
nuget install Microsoft.EntityFrameworkCore.Design
nuget install Microsoft.EntityFrameworkCore.Sqlite
```
## Реврс инжиниринг - Генерация кода
Команда состоит из:
   1. Точка входа ```dotnet ef dbcontext scaffold```
   2. Строка подключения ```"DataSource=Product.db"```
   3. Драйвер подключения зависит от баз данных которые вы используете ```Microsoft.EntityFrameworkCore.Sqlite```
   4. Дополнительные опции тк Расположение, Названия, список таблиц, форсирование и т.п. ``` -o Product -c ProductContext --table Developers --table Bugs --table Tasks -f```
```
   dotnet ef dbcontext scaffold "DataSource=Product.db" Microsoft.EntityFrameworkCore.Sqlite -o Product -c ProductContext --table Developers --table Bugs --table Tasks -f
```
## Разбираемся что получлось

   У генерации данных таким обазом есть ряд своих приемуществ.
   1. Переносит струтуру таблиц в классы.
   2. Переносит связи таблиц, если они созданы на слое базы данных.
   3. Генерирует составные(partial) классы для создания расширений.

   ```c#
   // ScaffoldStarter.Domain/Product/Bug.cs

    public partial class Bug
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public long? DeveloperId { get; set; }
        public long? Status { get; set; }
    }
   ```

   ```c#
   // ScaffoldStarter.Domain/Product/BugPartial.cs
   
   public partial class Bug
   {
       public virtual Developer Developer { get; set; }
   }
   ```

   4. Генерирует контексты
   5. Контексты для удобства расширения также создаются partial.

   ```c#
   // ScaffoldStarter.Domain/Product/ProductContextPartial.cs
   
    public partial class ProductContext
    {
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Bug>(entity =>
            {
                entity.HasOne(bug => bug.Developer)
                    .WithMany(developer => developer.Bugs)
                    .HasForeignKey(bug => bug.DeveloperId);
            });
        }
    }
   ```

   5. Написанные вами расширения не будут пропадать при обновлении контекста. Обновятся только сгенерированные файлы

         `ScaffoldStarter.Domain/Product/Bug.cs`  
         ScaffoldStarter.Domain/Product/BugPartial.cs  
         `// ScaffoldStarter.Domain/Product/ProductContext.cs`  
         ScaffoldStarter.Domain/Product/ProductContextPartial.cs  
