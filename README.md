# DPModule v 10.0
> CRC32: 65E87ABF

Deployment packages utility for RD AAOW FUPL projects. This tool performs installation / uninstallation / building / updates checking for our products

Модуль развёртывания для проектов RD AAOW FUPL. Инструмент выполняет установку / удаление / сборку / проверку обновлений для наших продуктов



## Изменения

- Программа, как и ранее, использует стандартные имена пакетов, если они расположены в той же директории, что и она сама. Однако при их отсутствии (т.е. если папка не содержит ни одного пакета) пользователь теперь может самостоятельно выбрать нужный файл пакета в любом удобном расположении. Для этого при запуске будет отображено соответствующее диалоговое окно.
- Окно ручного выбора пакета развёртки доступно также при вызове утилиты с ключом ```–m```.
- Выбор языка интерфейса, как и ранее, доступен при первом запуске и при запуске с ключом ```–l```. Однако при переходе на версию 9 его потребуется задать заново.
- Также при применении обновлений предыдущих версий (```dpu```) может потребоваться восстановление путей установки соответствующих модов. Для этого следует запустить программу с ключом ```–r```, выбрать пакет, для которого требуется задать путь установки, и указать его в соответствующем диалоговом окне.
- Значок в трее, отображаемый при запуске приложения, теперь поддерживает все функции, связанные с обновлением обрабатываемого пакета, по щелчку правой кнопкой мыши.



## Changes

- Application, as before, uses standard package names if they are located in the same directory with the executable. However, if they are absent (i.e. if the folder doesn't contain any packages), user can now independently select the needed package file in any convenient location. To do this, at startup, the corresponding dialog box will be displayed.
- Dialog box for manual selection of deployment package is also available when calling the utility with ```–m``` key.
- The choice of interface language, as before, is available at the first start and at start with ```–l``` key. However, when upgrading to version 9, you will need to set it again.
- Also, when applying updates from previous versions (```dpu```), it may be necessary to restore installation paths of corresponding mods. To do this, run the application with ```–r``` key, select the package for which you want to specify an installation path, and specify it in corresponding dialog box.
- Tray icon that is displayed when the application starts, now supports all functions related to updating the processed package, by right-clicking.



## Requirements / Требования

Needs Windows XP and newer, Framework 4.0 and newer. Interface languages: ru_ru, en_us

Требуется ОС Windows XP и новее, Framework 4.0 и новее. Языки интерфейса: ru_ru, en_us



## Development policy and EULA / Политика разработки и EULA

This [Policy (ADP)](https://vk.com/@rdaaow_fupl-adp), its positions, conclusion, EULA and application methods
describes general rules that we follow in all of our development processes, released applications and implemented
ideas.
**It must be acquainted by participants and users before using any of laboratory's products.
By downloading them, you agree to this Policy**

Данная [Политика (ADP)](https://vk.com/@rdaaow_fupl-adp), её положения, заключение, EULA и способы применения
описывают общие правила, которым мы следуем во всех наших процессах разработки, вышедших в релиз приложениях
и реализованных идеях.
**Обязательна к ознакомлению всем участникам и пользователям перед использованием любого из продуктов лаборатории.
Загружая их, вы соглашаетесь с этой Политикой**
