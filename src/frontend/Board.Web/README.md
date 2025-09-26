# Kanban Board - Angular Application

A modern Angular web application for managing tasks and projects using the Kanban methodology.

## 🚀 Features

- **Управление задачами**: Создание, редактирование и удаление задач
- **Канбан-доска**: Визуальное представление задач в колонках по статусам
- **Перемещение задач**: Drag & Drop функциональность между колонками
- **Приоритеты**: Система приоритетов (Низкий, Средний, Высокий, Срочный)
- **Назначение исполнителей**: Возможность назначить задачи конкретным людям
- **Сроки выполнения**: Установка и отслеживание дедлайнов
- **Теги**: Категоризация задач с помощью тегов
- **Статистика**: Отображение прогресса и статистики проекта
- **Адаптивный дизайн**: Полная поддержка мобильных устройств

## 📋 Статусы задач

1. **К выполнению** (TODO) - Новые задачи
2. **В работе** (IN_PROGRESS) - Задачи в процессе выполнения
3. **На проверке** (REVIEW) - Задачи на стадии проверки
4. **Завершено** (DONE) - Выполненные задачи

## 🛠 Технологии

- **Angular 15+** - Основной фреймворк
- **TypeScript** - Язык программирования
- **SCSS** - Препроцессор стилей
- **Material Icons** - Иконки
- **Reactive Forms** - Формы для создания/редактирования задач

## 📁 Project structure

```
src/
├── app/
│   ├── components/
│   │   ├── board/                 # Главный компонент доски
│   │   ├── board-column/          # Компонент колонки
│   │   ├── task-card/             # Компонент карточки задачи
│   │   └── task-modal/            # Модальное окно создания/редактирования
│   ├── models/
│   │   └── task.model.ts          # Модели данных и интерфейсы
│   ├── services/
│   │   ├── api-services/          # HTTP API services (Board, Board Columns, Board Items, Users)
│   │   └── mappers/               # Mapping helpers from API models to UI models
│   ├── app.component.*            # Главный компонент приложения
│   ├── app.module.ts              # Главный модуль
│   └── app-routing.module.ts      # Роутинг
├── assets/                        # Статические ресурсы
├── styles.scss                    # Глобальные стили
└── index.html                     # Главная HTML страница
```

## 🚀 Установка и запуск

### Предварительные требования

- Node.js (версия 16 или выше)
- npm или yarn

### Установка зависимостей

```bash
npm install
```

### Запуск в режиме разработки

```bash
ng serve
```

Приложение будет доступно по адресу `http://localhost:4200/`

### Сборка для продакшена

```bash
ng build
```

## 📱 Usage

### Создание новой задачи

1. Нажмите кнопку "+" в любой колонке
2. Заполните форму:
   - **Название** (обязательно)
   - **Описание** (опционально)
   - **Статус** (автоматически устанавливается в зависимости от колонки)
   - **Приоритет** (Низкий, Средний, Высокий, Срочный)
   - **Исполнитель** (опционально)
   - **Срок выполнения** (опционально)
   - **Теги** (через запятую)
3. Нажмите "Создать"

### Редактирование задачи

1. Нажмите на иконку редактирования (карандаш) на карточке задачи
2. Внесите необходимые изменения
3. Нажмите "Сохранить"

### Перемещение задачи

1. Нажмите кнопку "Переместить" на карточке задачи
2. Выберите новый статус из выпадающего списка

### Удаление задачи

1. Нажмите на иконку удаления (корзина) на карточке задачи
2. Подтвердите удаление

## 🎨 Design features

- **Современный UI**: Минималистичный и интуитивно понятный интерфейс
- **Цветовая схема**: Градиентный фон и цветовая кодировка статусов
- **Анимации**: Плавные переходы и hover-эффекты
- **Адаптивность**: Полная поддержка мобильных устройств
- **Доступность**: Поддержка клавиатурной навигации и screen readers

## 🔧 Configuration

### Task statuses configuration

Task statuses can be configured in `src/app/models/task.model.ts`: 

```typescript
export const TASK_STATUS_CONFIG: TaskStatusConfig[] = [
  {
    status: TaskStatus.TODO,
    label: 'К выполнению',
    color: '#e3f2fd',
    icon: 'assignment'
  },
  // ... другие статусы
];
```

### Priority configuration

Priorities are configured in the same file:

```typescript
export enum TaskPriority {
  LOW = 'low',
  MEDIUM = 'medium',
  HIGH = 'high',
  URGENT = 'urgent'
}
```

## 🚀 Extending functionality

### Adding new statuses

1. Добавьте новый статус в enum `TaskStatus`
2. Добавьте конфигурацию в `TASK_STATUS_CONFIG`
3. Обновите компонент доски для отображения новой колонки

### Backend integration

1. Create API services in `src/app/services/api-services` (e.g., `BoardApiService`, `BoardColumnApiService`, `BoardItemApiService`, `UsersApiService`)
2. Inject API services into components and orchestrate data flows via observables
3. Add error handling and loading states using RxJS operators

### Adding filtering

1. Create a filter component
2. Implement filtering on the API layer (query params) or in a dedicated state layer
3. Update the board component to apply filters via API service calls

## 📄 License

This project is created for educational purposes.

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add some amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## 📞 Support

If you have questions or suggestions, please open an issue in the repository.
