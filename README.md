# Helpdesk Remote Control

Утилита для поиска пользователей, компьютеров и запуска инструментов удаленного управления.

# Возможности

Поиск пользователей в Active Directory по имени или логину.

Отображение информации о пользователе из Active Directory.

Поиск компьютеров в System Center Configuration Manager по логину последнего входившего пользователя.

Отображение информации о компьютере из Configuration Manager и Active Directory.

Запуск инструментов удаленного управления компьютером:
- Ping
- Удаленная сессия Powershell
- Управление компьютером
- SCCM Remote Control (без необходимости запускать консоль SCCM)
- Удаленный помощник
- Удаленный рабочий стол
	 
# Требования

- .Net Framework 4.5
- Active Directory
- System Center Configuration Manager Current Branch (тестировалось на версии 1706, 1802)

# Настройка

В файле Settings.xml необходимо указать имя или адрес сервера Configuration Manager и путь для поиска пользователей в Active Directory.

# Заметки

Пароль локального администратора будет отображаться только если используется LAPS.
