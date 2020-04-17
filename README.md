# Helpdesk Remote Control

Утилита для поиска пользователей, компьютеров и запуска инструментов удаленного управления.

# Возможности

Поиск пользователей в Active Directory по имени или логину.

Отображение информации о пользователе из Active Directory.

Поиск компьютеров в System Center Configuration Manager по логину последнего входившего пользователя.

Поиск компьютеров в Active Directory по имени или описанию.

Отображение информации о компьютере из Configuration Manager и Active Directory.

Запуск инструментов удаленного управления компьютером:
- Ping
- Удаленная сессия CMD (с помощью psexec)
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

В файле Settings.xml необходимо указать имя или адрес сервера Configuration Manager и путь для поиска в Active Directory.

# Добавление инструментов удаленного управления

Сторонние инструменты удаленного управления (SCCM Remote Control, psexec) необходимо добавить самостоятельно.

Для использования psexec необходимо скопировать файл PsExec.exe из пакета Sysinternals (https://docs.microsoft.com/en-us/sysinternals) в каталог утилиты.

Для использования SCCM Remote Control необходимо скопировать несколько файлов из каталога установленной консоли SCCM (ниже приведен путь по умолчанию) в каталог RemoteControl:
- "C:\Program Files (x86)\Microsoft Configuration Manager\AdminConsole\bin\i386\00000409\CmRcViewerRes.dll" -> "RemoteControl\00000409\CmRcViewerRes.dll"
- "C:\Program Files (x86)\Microsoft Configuration Manager\AdminConsole\bin\i386\00000419\CmRcViewerRes.dll" -> "RemoteControl\00000419\CmRcViewerRes.dll"
- "C:\Program Files (x86)\Microsoft Configuration Manager\AdminConsole\bin\i386\CmRcViewer.exe" -> "RemoteControl\CmRcViewer.exe"
- "C:\Program Files (x86)\Microsoft Configuration Manager\AdminConsole\bin\i386\RdpCoreSccm.dll" -> "RemoteControl\RdpCoreSccm.dll"

# Заметки

Пароль локального администратора будет отображаться только если используется LAPS.
