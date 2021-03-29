# Стартовый набор для изучения dotnet Entity Framework Scaffold 

![File not found](http://www.plantuml.com/plantuml/proxy?cache=no&src=https://raw.githubusercontent.com/flirig/ScaffoldStarterSln/main/sln.txt)

## Entity Framework
1. ORM (Object Relation Mapping) это связывание Объектов вашего ЯП с реляционными БД. Основными компонентами современного ORM EF являются Entity & Context
2. Entity - Класс или экземпляр класса представляющего схему таблицы и одиночное вхождение в нее
3. Context - Класс содержащий набор коллекций энтити из БД и набор Апи для работы с ней. Позволяет удобно работать с Базой как со Множеством объектов к примеру используя синтаксис запросов LINQ.

## Установка dotnet sdk
![Microsoft](http://qrcoder.ru/code/?https%3A%2F%2Fdocs.microsoft.com%2Fru-ru%2Fdotnet%2Fcore%2Finstall%2F&6&0)

## Установка Scaffold
[Microsoft](https://docs.microsoft.com/ru-ru/ef/core/get-started/overview/install)  
![](http://qrcoder.ru/code/?https%3A%2F%2Fdocs.microsoft.com%2Fru-ru%2Fef%2Fcore%2Fget-started%2Foverview%2Finstall%23get-the-net-core-cli-tools&4&0)  

```bash
dotnet tool install --global dotnet-ef
```
## Добавление пакетов в проект ScaffoldStarter.Domain
Запускаем Cli в директории проекта
```bash
nuget install Microsoft.EntityFrameworkCore.Design
nuget install Microsoft.EntityFrameworkCore.Sqlite
```
## Реврс инжиниринг - Генерация кода
Запускаем Cli в директории проекта
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
   `ScaffoldStarter.Domain/Product/ProductContext.cs`  
   ScaffoldStarter.Domain/Product/ProductContextPartial.cs


## Пример добавления и обновления записи в БД.

   ```c#
    [Fact]
    public void DeveloperName_Should_BeUpdated()
    {
        // Arrange
        var testDeveloper = _context.Developers.Add(
            new Developer
            {
                FullName = "Tester Testov",
                Tasks = new List<Task>(new []
                {
                    new Task
                    {
                        Title = "Task Title",
                        Status = 0,
                    }
                })
                
            });
        _context.SaveChanges();

        // Act
        _context.Database.ExecuteSqlRaw("UPDATE Developers SET FullName = 'Tester Testerov' WHERE Id=1");
        _context.SaveChanges();
        var developer = _context.Developers
                .AsNoTracking()
                .Include( developer => developer.Tasks)
                .SingleOrDefault();

        // Assert
        developer.FullName.Should().Be("Tester Testerov");
        developer.Tasks.Count().Should().Be(1);
    }
   ```

## Ссылка на GitHub
![https://github.com/flirig/ScaffoldStarterSln](http://qrcoder.ru/code/?https%3A%2F%2Fgithub.com%2Fflirig%2FScaffoldStarterSln&6&0)

