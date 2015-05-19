﻿# Helpdesk Remote Control

Утилита для поиска пользователей, компьютеров пользователей и запуска инструментов удаленного управления.

# Возможности

Поиск пользователей в Active Directory по имени или логину.

Поиск компьютеров в System Center Configuration Manager по логину последнего входившего пользователя.

Запуск инструментов удаленного управления компьютером:
- Ping
- Удаленная сессия Powershell
- Управление компьютером
- SCCM Remote Control (без необходимости запускать консоль SCCM)
- Удаленный помощник
- Удаленный рабочий стол
	 
# Требования

- .Net Framework 4
- Active Directory
- System Center Configuration Manager 2012

# Настройка

В файле Settings.xml необходимо указать имя или адрес сервера Configuration Manager и путь для поиска пользователей в Active Directory.