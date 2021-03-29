# Стартовый набор для изучения dotnet Entity Framework Scaffold 

![File not found](http://www.plantuml.com/plantuml/proxy?cache=no&src=https://raw.githubusercontent.com/flirig/ScaffoldStarterSln/main/sln.txt)

## Entity Framework
1. ORM (Object Relation Mapping) это связывание Объектов вашего ЯП с реляционными БД. Основными компонентами современного ORM EF являются Entity & Context
2. Entity - Класс или экземпляр класса представляющего схему таблицы и одиночное вхождение в нее
3. Context - Класс содержащий набор коллекций энтити из БД и набор Апи для работы с ней. Позволяет удобно работать с Базой как со Множеством объектов к примеру используя синтаксис запросов LINQ.

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

## Коментарии
Обратите внимание что при создании контекста для крупных БД повторяющиеся наименования таблиц могут несовсем корректно сгенерироваться,
в этом случае возможно стоит подумать над разбиением на несколько контекстов.
## Ссылка на GitHub
![https://github.com/flirig/ScaffoldStarterSln](http://qrcoder.ru/code/?https%3A%2F%2Fgithub.com%2Fflirig%2FScaffoldStarterSln&6&0)

