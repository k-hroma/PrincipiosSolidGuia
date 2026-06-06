using System;

namespace PrincipiosSOLID
{
  // ========================================================================
  // >>>>>>>>>>>>>> SINGLE RESPONSIBILITY PRINCIPLE (SRP) <<<<<<<<<<<<<<<<<<<<
  // ========================================================================

  /*
  DEFINICIÓN:
  "Una clase debe tener una única razón para cambiar."
  Una clase debe encargarse de UNA sola responsabilidad.
  */


  // ------------------------------------------------------------------------
  // EJEMPLO QUE NO CUMPLE SRP
  // ------------------------------------------------------------------------

  /*
  PROBLEMAS:
  La clase:
  - Maneja el contenido del mensaje.
  - Sabe enviar correos.
  - Sabe enviar SMS.
  Tiene múltiples responsabilidades.
  */

  public class MensajeroNoSRP
  {
    private string mensaje;

    public string Mensaje
    {
      get { return mensaje; }
      set { mensaje = value; }
    }

    public MensajeroNoSRP(string mensaje)
    {
      this.mensaje = mensaje;
    }

    public void EnviarCorreoElectronico(string destino, string asunto)
    {
      Console.WriteLine($"Enviando correo a: {destino}");
      Console.WriteLine($"Asunto: {asunto}");
      Console.WriteLine($"Mensaje: {mensaje}");
    }

    public void EnviarSMS(int numero)
    {
      Console.WriteLine($"Enviando SMS al número: {numero}");
      Console.WriteLine($"Mensaje: {mensaje}");
    }
  }


  // ------------------------------------------------------------------------
  // SOLUCIÓN
  // ------------------------------------------------------------------------

  /*
  La responsabilidad de enviar mensajes se separa en distintas clases.
  Ahora:
  - Mensajero coordina.
  - MensajeroSMS sabe enviar SMS.
  - MensajeroCorreoElectronico sabe enviar emails.
  */

  public class Mensajero
  {
    private string mensaje;

    public string Mensaje
    {
      get { return mensaje; }
      set { mensaje = value; }
    }

    public Mensajero(string mensaje)
    {
      this.mensaje = mensaje;
    }

    public void EnviarSMS()
    {
      MensajeroSMS sms = new MensajeroSMS();
      sms.Enviar(mensaje);
    }

    public void EnviarCorreoElectronico()
    {
      MensajeroCorreoElectronico correo =
          new MensajeroCorreoElectronico();

      correo.EnviarMail(mensaje);
    }
  }

  public class MensajeroSMS
  {
    public void Enviar(string mensaje)
    {
      Console.WriteLine($"SMS enviado: {mensaje}");
    }
  }

  public class MensajeroCorreoElectronico
  {
    public void EnviarMail(string mensaje)
    {
      Console.WriteLine($"Email enviado: {mensaje}");
    }
  }



  // ========================================================================
  // >>>>>>>>>>>>>> OPEN/CLOSED PRINCIPLE (OCP) <<<<<<<<<<<<<<<<<<<<<<<<<<<<<
  // ========================================================================

  /*
  DEFINICIÓN:
  "Las entidades de software deben estar abiertas para extensión pero cerradas para modificación."
  */

  // ------------------------------------------------------------------------
  // ABSTRACCIÓN
  // ------------------------------------------------------------------------

  /*
  La clase abstracta define el contrato.
  Cada tipo de cuenta decide cómo retirar dinero.
  */

  public abstract class AccountType
  {
    private float amount;

    public float Amount
    {
      get { return amount; }
      set { amount = value; }
    }

    private string name;

    public string Name
    {
      get { return name; }
      set { name = value; }
    }

    public AccountType(float amount, string name)
    {
      this.amount = amount;
      this.name = name;
    }

    public abstract string Withdraw(float amount);
  }


  // ------------------------------------------------------------------------
  // CLASE CERRADA A MODIFICACIÓN
  // ------------------------------------------------------------------------

  /*
  Account delega el comportamiento al tipo de cuenta.
  NO necesita modificarse para agregar nuevos tipos.
  */

  public class Account
  {
    private AccountType accountType;

    public AccountType AccountType
    {
      get { return accountType; }
      set { accountType = value; }
    }

    private float amount;

    public float Amount
    {
      get { return amount; }
      set { amount = value; }
    }

    public Account(AccountType accountType, float amount)
    {
      this.accountType = accountType;
      this.amount = amount;
    }

    // delega la llamada al objeto AccountType
    public string Withdraw()
    {
      return accountType.Withdraw(amount);
    }
  }


  // ------------------------------------------------------------------------
  // EXTENSIONES
  // ------------------------------------------------------------------------

  public class SavingAccount : AccountType
  {
    public SavingAccount(float amount, string name)
        : base(amount, name)
    {
    }

    public override string Withdraw(float extractionAmount)
    {
      return $"Extracción desde caja de ahorro: ${extractionAmount}";
    }
  }

  public class GiroAccount : AccountType
  {
    public GiroAccount(float amount, string name)
        : base(amount, name)
    {
    }

    public override string Withdraw(float extractionAmount)
    {
      return $"Extracción desde cuenta giro: ${extractionAmount}";
    }
  }



  // ========================================================================
  // >>>>>>>>>>>>>> LISKOV SUBSTITUTION PRINCIPLE (LSP) <<<<<<<<<<<<<<<<<<<<<
  // ========================================================================

  /*
  DEFINICIÓN:

  "Los objetos de una clase derivada deben poder sustituir
  a los objetos de la clase base sin romper el programa."
  */


  // ------------------------------------------------------------------------
  // EJEMPLO QUE NO CUMPLE LSP
  // ------------------------------------------------------------------------

  public class Pato
  {
    public virtual void Nadar() { }

    public virtual void Cuak() { }

    public virtual void Volar() { }
  }

  /*
  PROBLEMA:

  PatitoDeGoma hereda Volar()
  aunque NO puede volar.

  Si reemplazamos un Pato por PatitoDeGoma,
  el programa falla.
  */

  public class PatitoDeGoma : Pato
  {
    public override void Nadar() { }

    public override void Volar()
    {
      throw new NotImplementedException();
    }
  }


  // ------------------------------------------------------------------------
  // SOLUCIÓN
  // ------------------------------------------------------------------------

  /*
  Se separan capacidades en interfaces específicas.
  */

  public interface INadador
  {
    void Nadar();
  }

  public interface ICuack
  {
    void Cuak();
  }

  public interface IVolador
  {
    void Volar();
  }

  public class PatoReal : INadador, ICuack, IVolador
  {
    public void Nadar()
    {
      Console.WriteLine("El pato nada.");
    }

    public void Cuak()
    {
      Console.WriteLine("Cuak.");
    }

    public void Volar()
    {
      Console.WriteLine("El pato vuela.");
    }
  }

  public class PatitoDeGomaCorrecto : INadador, ICuack
  {
    public void Nadar()
    {
      Console.WriteLine("El patito de goma flota.");
    }

    public void Cuak()
    {
      Console.WriteLine("Cuak de goma.");
    }
  }



  // ========================================================================
  // >>>>>>>>>>>>>> INTERFACE SEGREGATION PRINCIPLE (ISP) <<<<<<<<<<<<<<<<<<<
  // ========================================================================

  /*
  DEFINICIÓN:

  "Ningún cliente debe verse forzado
  a depender de métodos que no utiliza."
  */


  // ------------------------------------------------------------------------
  // EJEMPLO QUE NO CUMPLE ISP
  // ------------------------------------------------------------------------

  public abstract class Proceso
  {
    public abstract void Finalizar();

    public abstract void Iniciar();

    public abstract void Reanudar();

    public abstract void Suspender();
  }

  /*
  PROBLEMA:

  Automatico está obligado a implementar
  métodos manuales que no necesita.
  */

  public class Manual : Proceso
  {
    public override void Finalizar() { }

    public override void Iniciar() { }

    public override void Reanudar() { }

    public override void Suspender() { }
  }

  public class Automatico : Proceso
  {
    public override void Finalizar() { }

    public override void Iniciar() { }

    public override void Reanudar() { }

    public override void Suspender() { }
  }


  // ------------------------------------------------------------------------
  // SOLUCIÓN
  // ------------------------------------------------------------------------

  /*
  Se separan interfaces grandes
  en interfaces pequeñas y específicas.
  */

  public interface IManual
  {
    void Reanudar();

    void Suspender();
  }

  public interface IAutomatico
  {
    void Finalizar();

    void Iniciar();
  }

  public class ProcesoManual : IAutomatico, IManual
  {
    public void Reanudar()
    {
      Console.WriteLine("Proceso manual reanudado.");
    }

    public void Suspender()
    {
      Console.WriteLine("Proceso manual suspendido.");
    }

    public void Finalizar()
    {
      Console.WriteLine("Proceso manual finalizado.");
    }

    public void Iniciar()
    {
      Console.WriteLine("Proceso manual iniciado.");
    }
  }

  public class ProcesoAutomatico : IAutomatico
  {
    public void Finalizar()
    {
      Console.WriteLine("Proceso automático finalizado.");
    }

    public void Iniciar()
    {
      Console.WriteLine("Proceso automático iniciado.");
    }
  }



  // ========================================================================
  // >>>>>>>>>>>>>> DEPENDENCY INVERSION PRINCIPLE (DIP) <<<<<<<<<<<<<<<<<<<<
  // ========================================================================

  /*
  DEFINICIÓN FORMAL:

  1) Los módulos de alto nivel no deben depender
     de módulos de bajo nivel.

     Ambos deben depender de abstracciones.

  2) Las abstracciones no deben depender
     de los detalles.

     Los detalles deben depender de las abstracciones.
  */


  // ------------------------------------------------------------------------
  // EJEMPLO QUE NO CUMPLE DIP
  // ------------------------------------------------------------------------

  public class StandardMonitor { }

  public class StandardKeyboard { }

  /*
  PROBLEMA:

  Computer depende directamente
  de implementaciones concretas.
  */

  public class Computer
  {
    private readonly StandardKeyboard keyboard;
    private readonly StandardMonitor monitor;

    public Computer()
    {
      monitor = new StandardMonitor();
      keyboard = new StandardKeyboard();
    }
  }


  // ------------------------------------------------------------------------
  // SOLUCIÓN
  // ------------------------------------------------------------------------

  /*
  ComputerV2 depende de abstracciones
  y no de implementaciones concretas.
  */

  public interface IKeyboard
  {
    void Type(string text);
  }

  public interface IMonitor
  {
    void Display(string content);
  }

  public class StandardKeyboardV2 : IKeyboard
  {
    public void Type(string text)
    {
      Console.WriteLine($"Typing: {text}");
    }
  }

  public class StandardMonitorV2 : IMonitor
  {
    public void Display(string content)
    {
      Console.WriteLine($"Displaying: {content}");
    }
  }

  public class ComputerV2
  {
    private readonly IKeyboard keyboard;
    private readonly IMonitor monitor;

    public ComputerV2(IKeyboard keyboard, IMonitor monitor)
    {
      this.keyboard = keyboard;
      this.monitor = monitor;
    }
  }
}