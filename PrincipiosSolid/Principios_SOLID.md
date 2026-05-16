# Principios SOLID en C#

Guía práctica con ejemplos de código para entender y aplicar los 5 principios SOLID.

---

## 1. Single Responsibility (Responsabilidad Única)

> **"Una clase debe tener una, y solo una, razón para cambiar."**

### ❌ Problema

La clase `Book` tiene múltiples responsabilidades: gestionar datos del libro **y** manejar la impresión. Si queremos enviar por email, imprimir en papel, etc., debemos modificar `Book` por algo que no tiene que ver con autor, título o contenido.

```csharp
public class Book
{
    private string name;
    private string author;
    private string text;

    // Constructor
    public Book(string name, string author, string text)
    {
        this.name = name;
        this.author = author;
        this.text = text;
    }

    // Propiedades
    public string Name
    {
        get => name;
        set => name = value;
    }

    public string Author
    {
        get => author;
        set => author = value;
    }

    public string Text
    {
        get => text;
        set => text = value;
    }

    // Reemplazar palabra en el texto
    public string ReplaceWordInText(string word)
    {
        return text.Replace(word, text);
    }

    // Verificar si una palabra está en el texto
    public bool IsWordInText(string word)
    {
        return text.Contains(word);
    }

    // Imprimir el texto en consola
    public void PrintTextToConsole()
    {
        Console.WriteLine(text);
    }
}
```

### ✅ Solución

Extraer la responsabilidad de impresión a una clase dedicada `BookPrinter`:

```csharp
public class Book
{
    private string name;
    private string author;
    private string text;

    // Constructor
    public Book(string name, string author, string text)
    {
        this.name = name;
        this.author = author;
        this.text = text;
    }

    // Propiedades
    public string Name
    {
        get => name;
        set => name = value;
    }

    public string Author
    {
        get => author;
        set => author = value;
    }

    public string Text
    {
        get => text;
        set => text = value;
    }

    // Reemplazar palabra en el texto
    public string ReplaceWordInText(string word)
    {
        return text.Replace(word, text);
    }

    // Verificar si una palabra está en el texto
    public bool IsWordInText(string word)
    {
        return text.Contains(word);
    }
}

public class BookPrinter
{
    // Imprimir texto en consola
    public void PrintTextToConsole(string text)
    {
        Console.WriteLine(text);
    }

    // Simulación de impresión en otro medio
    public void PrintTextToAnotherMedium(string text)
    {
        Console.WriteLine($"[Otro medio] {text}");
    }
}
```

---

## 2. Open/Closed (Abierto/Cerrado)

> **"Las entidades de software deben estar abiertas para la extensión, pero cerradas para la modificación."**

### ❌ Problema

La clase `BookPersistence` solo guarda en archivo de texto. Si luego necesitamos persistir en base de datos, agregar un nuevo método rompe el principio y obliga a retestear código ya funcionando.

```csharp
public class BookPersistence
{
    // Guardar el texto del libro en un archivo
    public void SaveTextToFile(Book book, string filePath)
    {
        try
        {
            File.WriteAllText(filePath, book.Text);
            Console.WriteLine($"Texto guardado en {filePath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error al guardar el archivo: {ex.Message}");
        }
    }
}
```

### ✅ Solución

Utilizar una interfaz para permitir extender sin modificar:

```csharp
public interface IBookPersistence
{
    void Save(Book book);
}

public class TextFilePersistence : IBookPersistence
{
    public void Save(Book book)
    {
        string filePath = $"{book.Name}.txt";
        File.WriteAllText(filePath, book.Text);
        Console.WriteLine($"Texto del libro guardado en archivo: {filePath}");
    }
}

public class DbPersistence : IBookPersistence
{
    public void Save(Book book)
    {
        Console.WriteLine($"Texto del libro '{book.Name}' guardado en la base de datos.");
    }
}
```

**Nota:** La refactorización tiene costo. Si de entrada se hubiese arrancado con una interfaz, hubiese sido más fácil agregar nuevos métodos después.

---

## 3. Liskov Substitution (Sustitución de Liskov)

> **"Los objetos de una clase derivada deben poder sustituir a objetos de la clase base sin alterar el comportamiento correcto del programa."**

### ❌ Problema

`DumbDog` hereda de `Dog`, pero lanza una excepción en `MakeNoise()`. Si reemplazamos `Dog` por `DumbDog`, la aplicación deja de funcionar.

```csharp
using System;

public abstract class Animal
{
    public abstract void MakeNoise();
}

public class Dog : Animal
{
    public override void MakeNoise()
    {
        Console.WriteLine("bow wow");
    }
}

public class Cat : Animal
{
    public override void MakeNoise()
    {
        Console.WriteLine("meow meow");
    }
}

public class DumbDog : Dog
{
    public override void MakeNoise()
    {
        throw new Exception("I can't make noise");
    }
}

class Program
{
    static void Main()
    {
        Animal dog = new Dog();
        dog.MakeNoise(); // bow wow

        Animal cat = new Cat();
        cat.MakeNoise(); // meow meow

        Animal dumbDog = new DumbDog();
        try
        {
            dumbDog.MakeNoise(); // lanza excepción
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}
```

### 💡 Conclusión

`DumbDog` no debería extender `Dog`. Debería compartir otra interfaz, porque no es realmente un animal que haga ruido. Implementar un método solo porque está definido en la clase padre genera código fragil que requiere `try/catch`.

---

## 4. Interface Segregation (Segregación de Interfaces)

> **"Ningún cliente debe verse forzado a depender de métodos que no utiliza."**

### ✅ Solución

Subdividir interfaces grandes en otras más pequeñas para que cada clase implemente solo lo que necesita.

```csharp
using System;

public interface IBookPrinter
{
    void Print(string text);
}

public interface IPaperBookPrinter
{
    bool HasPaper();
}

public class BookPrinter : IBookPrinter
{
    public void Print(string text)
    {
        Console.WriteLine(text);
    }
}

public class PaperBookPrinter : IBookPrinter, IPaperBookPrinter
{
    private bool paperAvailable;

    public PaperBookPrinter(bool hasPaper)
    {
        paperAvailable = hasPaper;
    }

    public bool HasPaper()
    {
        return paperAvailable;
    }

    public void Print(string text)
    {
        if (HasPaper())
        {
            Console.WriteLine($"[Impresión en papel] {text}");
        }
        else
        {
            Console.WriteLine("No hay papel disponible para imprimir.");
        }
    }
}
```

---

## 5. Dependency Inversion (Inversión de Dependencias)

> **"Las clases deben depender de abstracciones y no de clases concretas."**

### ❌ Problema

La clase `Computer` crea instancias concretas de `StandardKeyboard` y `Monitor` internamente. Esto genera:

- **Alto acoplamiento** entre las clases.
- **Imposibilidad de cambiar** teclado o monitor sin modificar `Computer`.

```csharp
public class Computer
{
    private readonly StandardKeyboard keyboard;
    private readonly Monitor monitor;

    public Computer()
    {
        monitor = new Monitor();
        keyboard = new StandardKeyboard();
    }
}
```

### ✅ Solución

Inyectar dependencias a través de interfaces. Alguien desde afuera decide qué implementación pasar:

```csharp
public interface IKeyboard
{
    void Type(string text);
}

public interface IMonitor
{
    void Display(string content);
}

public class Computer
{
    private readonly IKeyboard keyboard;
    private readonly IMonitor monitor;

    public Computer(IKeyboard keyboard, IMonitor monitor)
    {
        this.keyboard = keyboard;
        this.monitor = monitor;
    }
}

public class StandardKeyboard : IKeyboard
{
    public void Type(string text)
    {
        Console.WriteLine($"Typing: {text}");
    }
}

public class Monitor : IMonitor
{
    public void Display(string content)
    {
        Console.WriteLine($"Displaying: {content}");
    }
}
```

### 💡 Ventajas

- Se oculta la implementación real de la clase A a la clase B.
- Si la clase A cambia, la clase B no necesita ser modificada.
- Facilita el testing unitario con mocks/stubs.

---

## Resumen

| Principio | Definición clave |
|-----------|------------------|
| **S**ingle Responsibility | Una clase, una razón para cambiar |
| **O**pen/Closed | Extiende sin modificar |
| **L**iskov Substitution | Las subclases deben ser intercambiables |
| **I**nterface Segregation | Interfaces pequeñas y específicas |
| **D**ependency Inversion | Depender de abstracciones, no concreciones |
