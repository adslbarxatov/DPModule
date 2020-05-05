# DPModule 9.2
> CRC32: 8B51ACE9

Deployment packages utility for RD AAOW FUPL projects. This tool performs installation / uninstallation / building / updates checking for our products

Модуль развёртки для проектов RD AAOW FUPL. Инструмент выполняет установку / удаление / сборку / проверку обновлений для наших продуктов

## Изменения

Новая версия программы обеспечивает весь функционал предыдущего релиза, несмотря на полный и глубокий пересмотр алгоритма её работы. В частности, модуль поддерживает пакеты версии 8 (```dpm``` / ```dpp``` / ```dpu```) и 8.6 (```dpx```). Вместе с тем модуль более не привязан к конкретным проектам и может быть использован для развёртки любого программного продукта или набора файлов. Кроме того, новая версия пакетов теперь поддерживает дополнительный установочный функционал.

Для новых пользователей нижеследующая информация в целом будет неактуальна за исключением следующего положения:

- При использовании установочных пакетов версии 8 в форматах ```dpm``` и ```dpp``` следует сначала выполнить установку пакета ```dpm```, а затем пакет ```dpp```. При наличии пакета обновления ```dpu``` его следует устанавливать после обоих пакетов установки. _Программа организует такой порядок установки автоматически, если пользователь не будет переименовывать файлы пакетов и разместит их в одной директории с модулем развёртки_.

Данная версия программы имеет следующие отличия:

- Программа, как и ранее, использует стандартные имена пакетов, если они расположены в той же директории, что и она сама. Однако при их отсутствии (т.е. если папка не содержит ни одного пакета) пользователь теперь может самостоятельно выбрать нужный файл пакета в любом удобном расположении. Для этого при запуске будет отображено соответствующее диалоговое окно.
- Окно ручного выбора пакета развёртки доступно также при вызове утилиты с ключом ```–m```.
- Выбор языка интерфейса, как и ранее, доступен при первом запуске и при запуске с ключом ```–l```. Однако при переходе на версию 9 его потребуется задать заново.
- Также при применении обновлений предыдущих версий (```dpu```) может потребоваться восстановление путей установки соответствующих модов. Для этого следует запустить программу с ключом ```–r```, выбрать пакет, для которого требуется задать путь установки, и указать его в соответствующем диалоговом окне.
- Значок в трее, отображаемый при запуске приложения, теперь поддерживает все функции, связанные с обновлением обрабатываемого пакета, по щелчку правой кнопкой мыши.

## Changes

New version of application provides all functionality of previous release, despite a complete and deep revision of the algorithm of its work. In particular, the module supports packages version 8 (```dpm``` / ```dpp``` / ```dpu```) and 8.6 (```dpx```). However, the module is no longer tied to specific projects and can be used to deploy any software product or set of files. In addition, new version of packages now supports additional installation functionality.

For new users, the following information will be irrelevant with the following exception:

- When using version 8 installation packages in ```dpm``` and ```dpp``` formats, first install the ```dpm``` package and then the ```dpp``` one. If the ```dpu``` update package is available too, it should be installed after both installation packages. _Application organizes this installation procedure automatically if user doesn't rename packages and places them in the same directory with the deployment module_.

This version of utility has following differences:

- Application, as before, uses standard package names if they are located in the same directory with the executable. However, if they are absent (i.e. if the folder doesn't contain any packages), user can now independently select the needed package file in any convenient location. To do this, at startup, the corresponding dialog box will be displayed.
- Dialog box for manual selection of deployment package is also available when calling the utility with ```–m``` key.
- The choice of interface language, as before, is available at the first start and at start with ```–l``` key. However, when upgrading to version 9, you will need to set it again.
- Also, when applying updates from previous versions (```dpu```), it may be necessary to restore installation paths of corresponding mods. To do this, run the application with ```–r``` key, select the package for which you want to specify an installation path, and specify it in corresponding dialog box.
- Tray icon that is displayed when the application starts, now supports all functions related to updating the processed package, by right-clicking.

#

We've formalized our [Applications development policy (ADP)](https://vk.com/@rdaaow_fupl-adp).
We're strongly recommend reading it before using our products.

Мы формализовали нашу [Политику разработки приложений (ADP)](https://vk.com/@rdaaow_fupl-adp).
Настоятельно рекомендуем ознакомиться с ней перед использованием наших продуктов.

#

Needs Windows XP and newer, Framework 4.0 and newer. Interface languages: ru_ru, en_us

Требуется ОС Windows XP и новее, Framework 4.0 и новее. Языки интерфейса: ru_ru, en_us
