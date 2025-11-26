using System;
using System.Collections.Generic;
using System.Linq;

namespace GeometryApp
{
    #region Інтерфейси

    /// <summary>
    /// Інтерфейс для об'єктів, які можуть обчислювати відстані
    /// </summary>
    public interface IDistanceCalculable
    {
        /// <summary>
        /// Обчислення відстані від точки до об'єкта
        /// </summary>
        double DistanceToPoint(params double[] point);

        /// <summary>
        /// Перевірка належності точки до об'єкта
        /// </summary>
        bool ContainsPoint(params double[] point);
    }

    /// <summary>
    /// Інтерфейс для об'єктів, які можуть бути валідовані
    /// </summary>
    public interface IValidatable
    {
        /// <summary>
        /// Перевірка валідності об'єкта
        /// </summary>
        bool IsValid();

        /// <summary>
        /// Отримання повідомлення про помилку валідації
        /// </summary>
        string GetValidationMessage();
    }

    /// <summary>
    /// Інтерфейс для об'єктів з коефіцієнтами
    /// </summary>
    public interface ICoefficientsManageable
    {
        /// <summary>
        /// Встановлення коефіцієнтів
        /// </summary>
        void SetCoefficients(params double[] coefficients);

        /// <summary>
        /// Отримання коефіцієнтів
        /// </summary>
        double[] GetCoefficients();

        /// <summary>
        /// Виведення коефіцієнтів
        /// </summary>
        void PrintCoefficients();
    }

    /// <summary>
    /// Інтерфейс для об'єктів, які можуть бути клоновані
    /// </summary>
    public interface IGeometryCloneable
    {
        /// <summary>
        /// Створення копії об'єкта
        /// </summary>
        GeometricObject Clone();
    }

    /// <summary>
    /// Інтерфейс для об'єктів, які можуть бути порівняні
    /// </summary>
    public interface IGeometryComparable
    {
        /// <summary>
        /// Порівняння двох геометричних об'єктів
        /// </summary>
        bool IsSimilar(GeometricObject other);
    }

    #endregion

    #region Абстрактний базовий клас

    /// <summary>
    /// Абстрактний базовий клас для всіх геометричних об'єктів
    /// Інкапсулює спільну логіку та властивості
    /// Реалізує всі основні інтерфейси
    /// </summary>
    public abstract class GeometricObject : 
        IDistanceCalculable, 
        IValidatable, 
        ICoefficientsManageable,
        IGeometryCloneable,
        IGeometryComparable
    {
        #region Захищені поля (інкапсуляція)

        protected const double EpsilonValue = 1e-10;
        private static int _instanceCounter = 0; // Лічильник екземплярів
        private readonly int _objectId; // Унікальний ID об'єкта

        #endregion

        #region Властивості (інкапсуляція)

        /// <summary>
        /// Унікальний ідентифікатор об'єкта (тільки для читання)
        /// </summary>
        public int ObjectId => _objectId;

        /// <summary>
        /// Загальна кількість створених об'єктів
        /// </summary>
        public static int TotalInstancesCreated => _instanceCounter;

        #endregion

        #region Конструктори та деструктор

        /// <summary>
        /// Конструктор базового класу
        /// Ініціалізує унікальний ID та збільшує лічильник
        /// </summary>
        protected GeometricObject()
        {
            _instanceCounter++;
            _objectId = _instanceCounter;
            
            Console.ForegroundColor = ConsoleColor. Cyan;
            Console.WriteLine($"[Конструктор] Створено {GetType().Name} #{_objectId}");
            Console.ResetColor();
        }

        /// <summary>
        /// Деструктор (фіналізатор)
        /// Викликається збирачем сміття перед знищенням об'єкта
        /// </summary>
        ~GeometricObject()
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console. WriteLine($"[Деструктор] Знищено {GetType().Name} #{_objectId}");
            Console.ResetColor();
        }

        #endregion

        #region Абстрактні методи (повинні бути реалізовані в похідних класах)

        /// <summary>
        /// Абстрактний метод для виведення інформації про об'єкт
        /// </summary>
        public abstract void PrintInfo();

        /// <summary>
        /// Абстрактний метод для отримання розмірності простору об'єкта
        /// </summary>
        public abstract int GetDimension();

        /// <summary>
        /// Абстрактний метод для отримання типу об'єкта
        /// </summary>
        public abstract string GetObjectType();

        #endregion

        #region Реалізація інтерфейсів

        // IDistanceCalculable
        public abstract bool ContainsPoint(params double[] point);
        public abstract double DistanceToPoint(params double[] point);

        // IValidatable
        public abstract bool IsValid();
        public abstract string GetValidationMessage();

        // ICoefficientsManageable
        public abstract void SetCoefficients(params double[] coefficients);
        public abstract double[] GetCoefficients();
        public abstract void PrintCoefficients();

        // IGeometryCloneable
        public abstract GeometricObject Clone();

        // IGeometryComparable
        public abstract bool IsSimilar(GeometricObject other);

        #endregion

        #region Допоміжні методи

        /// <summary>
        /// Захищений метод для валідації розмірності точки
        /// </summary>
        protected void ValidatePointDimension(double[] point, int expectedDimension)
        {
            if (point == null)
            {
                throw new ArgumentNullException(nameof(point), "Координати точки не можуть бути null");
            }

            if (point.Length != expectedDimension)
            {
                throw new ArgumentException(
                    $"Для {GetObjectType()} потрібно рівно {expectedDimension} координат.  Надано: {point.Length}");
            }
        }

        /// <summary>
        /// Перевизначення ToString для зручного виводу
        /// </summary>
        public override string ToString()
        {
            return $"{GetObjectType()} #{ObjectId} ({GetDimension()}D)";
        }

        #endregion
    }

    #endregion

    #region Клас Pryama (Пряма)

    /// <summary>
    /// Клас для представлення прямої на площині
    /// Рівняння: a1*x + a2*y + a0 = 0
    /// Демонструє інкапсуляцію через приватні поля та публічні властивості
    /// </summary>
    public class Pryama : GeometricObject
    {
        #region Приватні поля (інкапсуляція)

        private double _a0;
        private double _a1;
        private double _a2;

        #endregion

        #region Публічні властивості (контрольований доступ)

        /// <summary>
        /// Властивість для доступу до коефіцієнта a0 (вільний член)
        /// Protected set - дозволяє зміну тільки в цьому класі та похідних
        /// </summary>
        public double A0
        {
            get => _a0;
            protected set => _a0 = value;
        }

        /// <summary>
        /// Властивість для доступу до коефіцієнта a1 (коефіцієнт при x)
        /// </summary>
        public double A1
        {
            get => _a1;
            protected set => _a1 = value;
        }

        /// <summary>
        /// Властивість для доступу до коефіцієнта a2 (коефіцієнт при y)
        /// </summary>
        public double A2
        {
            get => _a2;
            protected set => _a2 = value;
        }

        #endregion

        #region Конструктори

        /// <summary>
        /// Конструктор за замовчуванням
        /// </summary>
        public Pryama() : base()
        {
            _a0 = 0;
            _a1 = 0;
            _a2 = 0;
        }

        /// <summary>
        /// Конструктор з параметрами
        /// </summary>
        public Pryama(double a0, double a1, double a2) : base()
        {
            _a0 = a0;
            _a1 = a1;
            _a2 = a2;

            if (! IsValid())
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"⚠ Попередження: {GetValidationMessage()}");
                Console.ResetColor();
            }
        }

        /// <summary>
        /// Конструктор копіювання
        /// </summary>
        public Pryama(Pryama other) : base()
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            _a0 = other._a0;
            _a1 = other._a1;
            _a2 = other._a2;
        }

        #endregion

        #region Реалізація абстрактних методів

        public override void SetCoefficients(params double[] coefficients)
        {
            if (coefficients == null)
                throw new ArgumentNullException(nameof(coefficients));

            if (coefficients.Length != 3)
                throw new ArgumentException($"Для прямої потрібно 3 коефіцієнти. Надано: {coefficients.Length}");

            A0 = coefficients[0];
            A1 = coefficients[1];
            A2 = coefficients[2];

            if (!IsValid())
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"⚠ {GetValidationMessage()}");
                Console.ResetColor();
            }
        }

        public override double[] GetCoefficients()
        {
            return new double[] { A0, A1, A2 };
        }

        public override void PrintCoefficients()
        {
            Console.WriteLine("╔═══════════════════════════════════════════════════════════╗");
            Console.WriteLine($"║                    ПРЯМА #{ObjectId}                          ║");
            Console.WriteLine("╚═══════════════════════════════════════════════════════════╝");
            Console.WriteLine($"Рівняння: ({A1})*x + ({A2})*y + ({A0}) = 0");
            Console.WriteLine($"Коефіцієнти: a0={A0}, a1={A1}, a2={A2}");
        }

        public override bool ContainsPoint(params double[] point)
        {
            ValidatePointDimension(point, 2);
            double result = A1 * point[0] + A2 * point[1] + A0;
            return Math.Abs(result) < EpsilonValue;
        }

        public override double DistanceToPoint(params double[] point)
        {
            if (! IsValid())
                throw new InvalidOperationException(GetValidationMessage());

            ValidatePointDimension(point, 2);

            double numerator = Math.Abs(A1 * point[0] + A2 * point[1] + A0);
            double denominator = Math.Sqrt(A1 * A1 + A2 * A2);

            return numerator / denominator;
        }

        public override bool IsValid()
        {
            return Math.Abs(A1) > EpsilonValue || Math.Abs(A2) > EpsilonValue;
        }

        public override string GetValidationMessage()
        {
            if (!IsValid())
                return "Пряма невалідна: a1 та a2 не можуть бути одночасно нульовими";
            return "Пряма валідна";
        }

        public override void PrintInfo()
        {
            Console.WriteLine($"┌─ Тип: {GetObjectType()} (ID: {ObjectId})");
            Console.WriteLine($"│  Рівняння: ({A1})*x + ({A2})*y + ({A0}) = 0");
            Console.WriteLine($"│  Розмірність: {GetDimension()}D");
            Console. WriteLine($"└─ Статус: {(IsValid() ? "✓ Валідний" : "✗ Невалідний")}");
        }

        public override int GetDimension() => 2;

        public override string GetObjectType() => "Пряма";

        public override GeometricObject Clone()
        {
            return new Pryama(this);
        }

        public override bool IsSimilar(GeometricObject other)
        {
            if (other is Pryama pryama)
            {
                // Дві прямі подібні, якщо їх коефіцієнти пропорційні
                double ratio = 0;
                bool ratioSet = false;

                double[] thisCoeffs = GetCoefficients();
                double[] otherCoeffs = pryama.GetCoefficients();

                for (int i = 0; i < thisCoeffs.Length; i++)
                {
                    if (Math.Abs(thisCoeffs[i]) > EpsilonValue && Math.Abs(otherCoeffs[i]) > EpsilonValue)
                    {
                        double currentRatio = thisCoeffs[i] / otherCoeffs[i];
                        if (! ratioSet)
                        {
                            ratio = currentRatio;
                            ratioSet = true;
                        }
                        else if (Math.Abs(ratio - currentRatio) > EpsilonValue)
                        {
                            return false;
                        }
                    }
                }
                return ratioSet;
            }
            return false;
        }

        public override string ToString()
        {
            return $"Пряма #{ObjectId}: ({A1})*x + ({A2})*y + ({A0}) = 0";
        }

        #endregion
    }

    #endregion

    #region Клас Giperploschyna (Гіперплощина)

    /// <summary>
    /// Клас для гіперплощини у 4-вимірному просторі
    /// Рівняння: a1*x1 + a2*x2 + a3*x3 + a4*x4 + a0 = 0
    /// Наслідує Pryama та розширює до 4D
    /// </summary>
    public class Giperploschyna : Pryama
    {
        #region Приватні поля (інкапсуляція)

        private double _a3;
        private double _a4;

        #endregion

        #region Публічні властивості

        public double A3
        {
            get => _a3;
            protected set => _a3 = value;
        }

        public double A4
        {
            get => _a4;
            protected set => _a4 = value;
        }

        #endregion

        #region Конструктори

        /// <summary>
        /// Конструктор за замовчуванням
        /// Викликає конструктор базового класу
        /// </summary>
        public Giperploschyna() : base()
        {
            _a3 = 0;
            _a4 = 0;
        }

        /// <summary>
        /// Конструктор з параметрами
        /// </summary>
        public Giperploschyna(double a0, double a1, double a2, double a3, double a4) 
            : base(a0, a1, a2)
        {
            _a3 = a3;
            _a4 = a4;

            if (!IsValid())
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"⚠ Попередження: {GetValidationMessage()}");
                Console.ResetColor();
            }
        }

        /// <summary>
        /// Конструктор копіювання
        /// </summary>
        public Giperploschyna(Giperploschyna other) : base(other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            _a3 = other._a3;
            _a4 = other._a4;
        }

        #endregion

        #region Перевизначені методи

        public override void SetCoefficients(params double[] coefficients)
        {
            if (coefficients == null)
                throw new ArgumentNullException(nameof(coefficients));

            if (coefficients.Length != 5)
                throw new ArgumentException($"Для гіперплощини потрібно 5 коефіцієнтів. Надано: {coefficients.Length}");

            A0 = coefficients[0];
            A1 = coefficients[1];
            A2 = coefficients[2];
            A3 = coefficients[3];
            A4 = coefficients[4];

            if (!IsValid())
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console. WriteLine($"⚠ {GetValidationMessage()}");
                Console.ResetColor();
            }
        }

        public override double[] GetCoefficients()
        {
            return new double[] { A0, A1, A2, A3, A4 };
        }

        public override void PrintCoefficients()
        {
            Console.WriteLine("╔═══════════════════════════════════════════════════════════╗");
            Console. WriteLine($"║                ГІПЕРПЛОЩИНА #{ObjectId}                      ║");
            Console.WriteLine("╚═══════════════════════════════════════════════════════════╝");
            Console.WriteLine($"Рівняння: ({A1})*x1 + ({A2})*x2 + ({A3})*x3 + ({A4})*x4 + ({A0}) = 0");
            Console.WriteLine($"Коефіцієнти: a0={A0}, a1={A1}, a2={A2}, a3={A3}, a4={A4}");
        }

        public override bool ContainsPoint(params double[] point)
        {
            ValidatePointDimension(point, 4);
            double result = A1 * point[0] + A2 * point[1] + A3 * point[2] + A4 * point[3] + A0;
            return Math.Abs(result) < EpsilonValue;
        }

        public override double DistanceToPoint(params double[] point)
        {
            if (! IsValid())
                throw new InvalidOperationException(GetValidationMessage());

            ValidatePointDimension(point, 4);

            double numerator = Math. Abs(A1 * point[0] + A2 * point[1] + A3 * point[2] + A4 * point[3] + A0);
            double denominator = Math.Sqrt(A1 * A1 + A2 * A2 + A3 * A3 + A4 * A4);

            return numerator / denominator;
        }

        public override bool IsValid()
        {
            return Math. Abs(A1) > EpsilonValue || Math.Abs(A2) > EpsilonValue ||
                   Math.Abs(A3) > EpsilonValue || Math.Abs(A4) > EpsilonValue;
        }

        public override string GetValidationMessage()
        {
            if (!IsValid())
                return "Гіперплощина невалідна: всі коефіцієнти a1, a2, a3, a4 не можуть бути одночасно нульовими";
            return "Гіперплощина валідна";
        }

        public override void PrintInfo()
        {
            Console.WriteLine($"┌─ Тип: {GetObjectType()} (ID: {ObjectId})");
            Console.WriteLine($"│  Рівняння: ({A1})*x1 + ({A2})*x2 + ({A3})*x3 + ({A4})*x4 + ({A0}) = 0");
            Console.WriteLine($"│  Розмірність: {GetDimension()}D");
            Console.WriteLine($"└─ Статус: {(IsValid() ? "✓ Валідний" : "✗ Невалідний")}");
        }

        public override int GetDimension() => 4;

        public override string GetObjectType() => "Гіперплощина";

        public override GeometricObject Clone()
        {
            return new Giperploschyna(this);
        }

        public override bool IsSimilar(GeometricObject other)
        {
            if (other is Giperploschyna giper)
            {
                double ratio = 0;
                bool ratioSet = false;

                double[] thisCoeffs = GetCoefficients();
                double[] otherCoeffs = giper.GetCoefficients();

                for (int i = 0; i < thisCoeffs.Length; i++)
                {
                    if (Math.Abs(thisCoeffs[i]) > EpsilonValue && Math. Abs(otherCoeffs[i]) > EpsilonValue)
                    {
                        double currentRatio = thisCoeffs[i] / otherCoeffs[i];
                        if (!ratioSet)
                        {
                            ratio = currentRatio;
                            ratioSet = true;
                        }
                        else if (Math.Abs(ratio - currentRatio) > EpsilonValue)
                        {
                            return false;
                        }
                    }
                }
                return ratioSet;
            }
            return false;
        }

        public override string ToString()
        {
            return $"Гіперплощина #{ObjectId}: ({A1})*x1 + ({A2})*x2 + ({A3})*x3 + ({A4})*x4 + ({A0}) = 0";
        }

        #endregion
    }

    #endregion

    #region Менеджер геометрії

    /// <summary>
    /// Клас для управління колекцією геометричних об'єктів
    /// Демонструє роботу з інтерфейсами та поліморфізм
    /// </summary>
    public class GeometryManager
    {
        private List<GeometricObject> _objects;

        public GeometryManager()
        {
            _objects = new List<GeometricObject>();
            Console.ForegroundColor = ConsoleColor. Cyan;
            Console.WriteLine("[Конструктор] Створено GeometryManager");
            Console. ResetColor();
        }

        ~GeometryManager()
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($"[Деструктор] Знищено GeometryManager ({_objects.Count} об'єктів)");
            Console. ResetColor();
        }

        public void AddObject(GeometricObject obj)
        {
            if (obj != null)
            {
                _objects.Add(obj);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"✓ Додано: {obj}");
                Console.ResetColor();
            }
        }

        public void PrintAllObjects()
        {
            Console.WriteLine($"\n{UiConstants.BoxTop}");
            Console.WriteLine("║          СПИСОК ВСІХ ОБ'ЄКТІВ                             ║");
            Console.WriteLine($"{UiConstants.BoxBottom}\n");

            if (_objects.Count == 0)
            {
                Console.WriteLine("Список порожній.");
                return;
            }

            for (int i = 0; i < _objects.Count; i++)
            {
                Console.WriteLine($"[{i + 1}] {_objects[i]}");
            }
        }

        public void DemonstrateInterfaces()
        {
            Console.WriteLine($"\n{UiConstants.BoxTop}");
            Console.WriteLine("║         ДЕМОНСТРАЦІЯ РОБОТИ ІНТЕРФЕЙСІВ                   ║");
            Console.WriteLine($"{UiConstants.BoxBottom}\n");

            foreach (var obj in _objects)
            {
                Console.WriteLine($"\n{UiConstants. Separator}");
                Console.WriteLine($"Об'єкт: {obj}\n");

                // IValidatable
                if (obj is IValidatable validatable)
                {
                    Console.WriteLine($"📋 IValidatable:");
                    Console.WriteLine($"   IsValid(): {validatable.IsValid()}");
                    Console.WriteLine($"   Message: {validatable.GetValidationMessage()}");
                }

                // ICoefficientsManageable
                if (obj is ICoefficientsManageable coeffManageable)
                {
                    Console.WriteLine($"\n📊 ICoefficientsManageable:");
                    double[] coeffs = coeffManageable.GetCoefficients();
                    Console.WriteLine($"   Коефіцієнти: [{string.Join(", ", coeffs)}]");
                }

                // IDistanceCalculable - тестуємо з точкою
                if (obj is IDistanceCalculable distCalculable)
                {
                    Console.WriteLine($"\n📏 IDistanceCalculable:");
                    try
                    {
                        double[] testPoint = obj.GetDimension() == 2 
                            ? new double[] { 0, 0 } 
                            : new double[] { 0, 0, 0, 0 };
                        
                        bool contains = distCalculable.ContainsPoint(testPoint);
                        double distance = distCalculable.DistanceToPoint(testPoint);
                        
                        Console.WriteLine($"   Точка ({string.Join(", ", testPoint)}):");
                        Console.WriteLine($"   ContainsPoint(): {contains}");
                        Console.WriteLine($"   DistanceToPoint(): {distance:F6}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"   Помилка: {ex.Message}");
                    }
                }

                // IGeometryCloneable
                if (obj is IGeometryCloneable cloneable)
                {
                    Console. WriteLine($"\n🔄 IGeometryCloneable:");
                    GeometricObject clone = cloneable.Clone();
                    Console. WriteLine($"   Оригінал: {obj}");
                    Console.WriteLine($"   Клон: {clone}");
                    Console.WriteLine($"   Клон створено успішно!");
                }
            }
        }

        public void DemonstrateComparison()
        {
            Console.WriteLine($"\n{UiConstants.BoxTop}");
            Console.WriteLine("║         ДЕМОНСТРАЦІЯ ПОРІВНЯННЯ ОБ'ЄКТІВ                  ║");
            Console.WriteLine($"{UiConstants.BoxBottom}\n");

            // Створюємо подібні об'єкти
            Pryama p1 = new Pryama(1, 2, 3);
            Pryama p2 = new Pryama(2, 4, 6); // Подібна до p1 (коефіцієнти * 2)
            Pryama p3 = new Pryama(1, 1, 1); // Не подібна

            Console.WriteLine("Створено тестові об'єкти:");
            Console.WriteLine($"p1: {p1}");
            Console.WriteLine($"p2: {p2}");
            Console.WriteLine($"p3: {p3}\n");

            if (p1 is IGeometryComparable comp1)
            {
                Console.WriteLine($"p1. IsSimilar(p2): {comp1.IsSimilar(p2)} (очікується true - пропорційні коефіцієнти)");
                Console.WriteLine($"p1. IsSimilar(p3): {comp1.IsSimilar(p3)} (очікується false)");
            }
        }

        public void CheckPointForAll(double[] point)
        {
            Console.WriteLine($"\n{UiConstants.BoxTop}");
            Console.WriteLine($"║  ПЕРЕВІРКА ТОЧКИ ({string.Join(", ", point)})");
            Console.WriteLine($"{UiConstants.BoxBottom}\n");

            foreach (var obj in _objects)
            {
                int requiredDim = obj.GetDimension();
                if (point.Length != requiredDim)
                {
                    Console.ForegroundColor = ConsoleColor. Red;
                    Console.WriteLine($"{obj. GetObjectType()}: Невідповідна розмірність (потрібно {requiredDim}D)");
                    Console.ResetColor();
                    continue;
                }

                try
                {
                    bool belongs = obj.ContainsPoint(point);
                    double distance = obj.DistanceToPoint(point);

                    Console.ForegroundColor = belongs ? ConsoleColor.Green : ConsoleColor.Yellow;
                    Console.WriteLine($"{obj}: {(belongs ? "✓ НАЛЕЖИТЬ" : "✗ НЕ НАЛЕЖИТЬ")}");
                    Console.WriteLine($"  Відстань: {distance:F6}");
                    Console.ResetColor();
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"{obj}: Помилка - {ex.Message}");
                    Console.ResetColor();
                }
            }
        }

        public int GetObjectCount() => _objects.Count;
    }

    #endregion

    #region UI Константи

    public static class UiConstants
    {
        public const string BoxTop = "╔═══════════════════════════════════════════════════════════╗";
        public const string BoxBottom = "╚═══════════════════════════════════════════════════════════╝";
        public const string Separator = "────────────────────────────────────────────────────────────";
        public const string SectionTop = "┌─────────────────────────────────────────────────────────┐";
        public const string SectionBottom = "└─────────────────────────────────────────────────────────┘";
    }

    #endregion

    #region Input Helper

    public static class InputHelper
    {
        public static double ReadDouble(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                if (double.TryParse(Console.ReadLine(), out double result))
                    return result;

                Console.ForegroundColor = ConsoleColor. Red;
                Console.WriteLine("❌ Помилка!  Введіть коректне число.");
                Console.ResetColor();
            }
        }

        public static int ReadInt(string prompt, int minValue = int.MinValue)
        {
            while (true)
            {
                Console.Write(prompt);
                if (int.TryParse(Console.ReadLine(), out int result) && result >= minValue)
                    return result;

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"❌ Помилка! Введіть коректне число (мінімум {minValue}).");
                Console.ResetColor();
            }
        }

        public static int ReadDimension(string prompt)
        {
            while (true)
            {
                Console. Write(prompt);
                if (int.TryParse(Console.ReadLine(), out int result))
                {
                    if (result == 2 || result == 4)
                        return result;
                }

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("❌ Помилка! Підтримуються тільки розмірності 2 або 4.");
                Console.ResetColor();
            }
        }

        public static double[] ReadCoefficients(int count, string typeName)
        {
            double[] coefficients = new double[count];
            Console.WriteLine($"\n📝 Введіть {count} коефіцієнтів для {typeName} (a0, a1, a2{(count > 3 ? ", a3, a4" : "")}):");

            for (int i = 0; i < count; i++)
            {
                coefficients[i] = ReadDouble($"   a{i} = ");
            }

            return coefficients;
        }

        public static double[] ReadPoint(int dimension)
        {
            double[] point = new double[dimension];
            Console.WriteLine($"\n📍 Введіть координати точки ({dimension}D):");

            if (dimension == 2)
            {
                point[0] = ReadDouble("   x = ");
                point[1] = ReadDouble("   y = ");
            }
            else
            {
                for (int i = 0; i < dimension; i++)
                {
                    point[i] = ReadDouble($"   x{i + 1} = ");
                }
            }

            return point;
        }
    }

    #endregion

    #region Головна програма

    public class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text. Encoding.UTF8;

            PrintHeader();

            try
            {
                GeometryManager manager = new GeometryManager();

                Console.WriteLine($"\n{UiConstants.SectionTop}");
                Console.WriteLine("│ ЕТАП 1: Створення об'єктів (Конструктори)               │");
                Console.WriteLine($"{UiConstants.SectionBottom}\n");

                CreateObjects(manager);

                Console. WriteLine($"\n{UiConstants.SectionTop}");
                Console.WriteLine("│ ЕТАП 2: Демонстрація інтерфейсів                        │");
                Console.WriteLine($"{UiConstants.SectionBottom}");

                manager.DemonstrateInterfaces();

                Console.WriteLine($"\n{UiConstants.SectionTop}");
                Console.WriteLine("│ ЕТАП 3: Демонстрація порівняння (IGeometryComparable)   │");
                Console.WriteLine($"{UiConstants.SectionBottom}");

                manager.DemonstrateComparison();

                Console.WriteLine($"\n{UiConstants.SectionTop}");
                Console.WriteLine("│ ЕТАП 4: Демонстрація клонування (IGeometryCloneable)    │");
                Console.WriteLine($"{UiConstants.SectionBottom}\n");

                DemonstrateCloningAndEncapsulation();

                Console.WriteLine($"\n{UiConstants.SectionTop}");
                Console.WriteLine("│ ЕТАП 5: Перевірка точок                                  │");
                Console.WriteLine($"{UiConstants.SectionBottom}");

                CheckPointsLoop(manager);

                Console.WriteLine($"\n{UiConstants. SectionTop}");
                Console.WriteLine("│ ЕТАП 6: Статистика                                       │");
                Console.WriteLine($"{UiConstants.SectionBottom}\n");

                ShowStatistics(manager);

                Console.WriteLine($"\n{UiConstants.SectionTop}");
                Console.WriteLine("│ ЕТАП 7: Демонстрація інкапсуляції                       │");
                Console.WriteLine($"{UiConstants.SectionBottom}\n");

                DemonstrateEncapsulation();
            }
            catch (Exception ex)
            {
                Console. ForegroundColor = ConsoleColor. Red;
                Console.WriteLine($"\n❌ Критична помилка: {ex. Message}");
                Console.WriteLine($"Деталі: {ex.StackTrace}");
                Console.ResetColor();
            }

            PrintFooter();

            // Демонстрація деструкторів
            Console.WriteLine($"\n{UiConstants.SectionTop}");
            Console. WriteLine("│ ЕТАП 8: Демонстрація деструкторів (GC)                  │");
            Console.WriteLine($"{UiConstants.SectionBottom}\n");
            
            Console.WriteLine("Виклик збирача сміття.. .");
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            Console.ReadKey();
        }

        static void PrintHeader()
        {
            Console.WriteLine(UiConstants.BoxTop);
            Console.WriteLine("║  Лабораторна #3: Абстрактні класи та інтерфейси         ║");
            Console.WriteLine("║  Виконав: noic9                                           ║");
            Console.WriteLine("║  Дата: 2025-11-26                                         ║");
            Console.WriteLine(UiConstants.BoxBottom);
        }

        static void PrintFooter()
        {
            Console.WriteLine($"\n{UiConstants.BoxTop}");
            Console.WriteLine("║  Програма завершена.  Натисніть клавішу...                 ║");
            Console.WriteLine(UiConstants.BoxBottom);
        }

        static void CreateObjects(GeometryManager manager)
        {
            Console.WriteLine("🔹 Створення Пряма (2D):");
            GeometricObject pryama = new Pryama();
            double[] coeffPryama = InputHelper.ReadCoefficients(3, "прямої");
            pryama.SetCoefficients(coeffPryama);
            manager.AddObject(pryama);

            Console. WriteLine("\n🔹 Створення Гіперплощина (4D):");
            GeometricObject giper = new Giperploschyna();
            double[] coeffGiper = InputHelper.ReadCoefficients(5, "гіперплощини");
            giper.SetCoefficients(coeffGiper);
            manager.AddObject(giper);
        }

        static void CheckPointsLoop(GeometryManager manager)
        {
            int pointCount = InputHelper.ReadInt("\nВведіть кількість точок для перевірки: ", 0);

            for (int i = 0; i < pointCount; i++)
            {
                Console.WriteLine($"\n{new string('─', 60)}");
                Console.WriteLine($"Точка #{i + 1}:");

                int dimension = InputHelper.ReadDimension("Розмірність (2 або 4): ");
                double[] point = InputHelper. ReadPoint(dimension);
                manager.CheckPointForAll(point);
            }
        }

        static void ShowStatistics(GeometryManager manager)
        {
            Console.WriteLine($"📊 Статистика:");
            Console.WriteLine($"   Всього об'єктів у менеджері: {manager.GetObjectCount()}");
            Console. WriteLine($"   Всього створено екземплярів: {GeometricObject.TotalInstancesCreated}");
            Console. WriteLine($"   Реалізовано інтерфейсів: 5");
            Console.WriteLine($"   • IDistanceCalculable");
            Console.WriteLine($"   • IValidatable");
            Console.WriteLine($"   • ICoefficientsManageable");
            Console. WriteLine($"   • IGeometryCloneable");
            Console.WriteLine($"   • IGeometryComparable");
        }

        static void DemonstrateCloningAndEncapsulation()
        {
            Console.WriteLine("🔄 Клонування об'єктів:\n");

            Pryama original = new Pryama(1, 2, 3);
            Console.WriteLine($"Оригінал: {original}");

            // Клонування через інтерфейс
            IGeometryCloneable cloneable = original;
            GeometricObject clone = cloneable.Clone();
            
            Console.WriteLine($"Клон: {clone}");
            Console.WriteLine($"\nПеревірка незалежності:");
            
            // Змінюємо оригінал
            original.SetCoefficients(10, 20, 30);
            Console.WriteLine($"Після зміни оригіналу:");
            Console.WriteLine($"  Оригінал: {original}");
            Console.WriteLine($"  Клон: {clone}");
            Console.WriteLine($"  ✓ Клон залишився незмінним (глибоке копіювання)");
        }

        static void DemonstrateEncapsulation()
        {
            Console.WriteLine("🔒 Демонстрація інкапсуляції:\n");

            Pryama p = new Pryama(1, 2, 3);

            Console.WriteLine("1. Доступ до даних через властивості (get):");
            Console.WriteLine($"   A0 = {p.A0}");
            Console.WriteLine($"   A1 = {p.A1}");
            Console.WriteLine($"   A2 = {p.A2}");

            Console.WriteLine("\n2.  Зміна даних тільки через SetCoefficients:");
            Console.WriteLine("   (Прямий доступ p.A0 = 10 заборонений - protected set)");
            p.SetCoefficients(10, 20, 30);
            Console.WriteLine($"   Після SetCoefficients: {p}");

            Console.WriteLine("\n3. Readonly властивість ObjectId:");
            Console.WriteLine($"   ObjectId = {p.ObjectId} (тільки для читання)");

            Console.WriteLine("\n4. Static властивість TotalInstancesCreated:");
            Console.WriteLine($"   Всього створено: {GeometricObject.TotalInstancesCreated}");

            Console.WriteLine("\n✓ Інкапсуляція дотримана:");
            Console.WriteLine("  • Приватні поля (_a0, _a1, _a2)");
            Console.WriteLine("  • Публічні властивості з protected set");
            Console.WriteLine("  • Readonly властивості (ObjectId)");
            Console.WriteLine("  • Контрольоване встановлення через методи");
        }
    }

    #endregion
}
