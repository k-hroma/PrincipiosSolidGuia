using System;
using PrincipiosSOLID;

namespace AplicacionSOLID
{
  public class Program
  {
    public static void Main()
    {
      Console.WriteLine("========== SRP ==========\n");

      Mensajero mensajero =
          new Mensajero("Hola desde SOLID");

      mensajero.EnviarSMS();
      mensajero.EnviarCorreoElectronico();


      Console.WriteLine("\n========== OCP ==========\n");

      SavingAccount saving =
          new SavingAccount(1000, "Caja de ahorro");

      Account cuentaAhorro =
          new Account(saving, 200);

      Console.WriteLine(cuentaAhorro.Withdraw());

      GiroAccount giro =
          new GiroAccount(5000, "Cuenta Giro");

      Account cuentaGiro =
          new Account(giro, 1000);

      Console.WriteLine(cuentaGiro.Withdraw());


      Console.WriteLine("\n========== LSP ==========\n");

      PatoReal pato = new PatoReal();

      pato.Nadar();
      pato.Cuak();
      pato.Volar();

      Console.WriteLine();

      PatitoDeGomaCorrecto patitoGoma =
          new PatitoDeGomaCorrecto();

      patitoGoma.Nadar();
      patitoGoma.Cuak();


      Console.WriteLine("\n========== ISP ==========\n");

      ProcesoManual manual = new ProcesoManual();

      manual.Iniciar();
      manual.Reanudar();
      manual.Suspender();
      manual.Finalizar();

      Console.WriteLine();

      ProcesoAutomatico automatico =
          new ProcesoAutomatico();

      automatico.Iniciar();
      automatico.Finalizar();


      Console.WriteLine("\n========== DIP ==========\n");

      StandardKeyboardV2 keyboard =
          new StandardKeyboardV2();

      StandardMonitorV2 monitor =
          new StandardMonitorV2();

      keyboard.Type("Lenovo");

      monitor.Display("Monitor Lenovo");

      ComputerV2 computer =
          new ComputerV2(keyboard, monitor);

      Console.WriteLine("ComputerV2 creada correctamente.");
    }
  }
}