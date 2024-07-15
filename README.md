## Flashing Tool

This tool simplifies the process of installing new firmware, making it accessible even for users without extensive technical background.

### README Translated Versions

---

- [English](README.md)
- [中文](README.zh.md)

### Getting Started

---

The following instructions will help you obtain a copy of the project on your local machine and prepare it for development and testing.

### Prerequisites

---

You need to install the following software and ensure they are correctly installed:

- Windows 10 version 1803 or later.
- .NET Framework 4.7.2 or later must be installed on your machine.

### Installation Steps

---

The step-by-step examples below will guide you on how to set up the development environment:

1. Clone the repository or download the ZIP file.

```bash
git clone https://github.com/Ranshen1209/Ranshen-s-Flash.git
```

2. Navigate to the cloned or downloaded directory.

3. Build the project using Visual Studio.

4. After building the project, the `flashbin` directory will be configured with the necessary files and folders for operation.

```bash
/flashbin
|-- /platform-tools           # Contains Google's adb and fastboot executables
|-- /resources                # Contents required for the program to run
|-- /drivers                  # Contains device drivers, particularly from Xiaomi
```

### How to Apply

---

#### Driver Installation Script Example

```bash
@echo off
"C:\Windows\sysnative\pnputil.exe" /add-driver "%~dp0..\ThirdParty\Google\Driver\android_winusb.inf" /install
"C:\Windows\sysnative\pnputil.exe" /add-driver "%~dp0..\ThirdParty\Qualcomm\Driver\qcser.inf" /install
```

#### Line Flashing Script Example

```bash
@echo off
setlocal
set PATH=%~dp0flashbin\platform-tools;%~dp0flashbin\zstd;%PATH%

REM Check device model
for /f "tokens=2 delims=: " %%a in ('fastboot %* getvar product 2^>^&1 ^| findstr /r /c:"^product: *marble"') do (
    if /i not "%%a"=="marble" (
        echo Mismatching image and device
        exit /B 1
    )
)

REM Check and remove existing super.img if exists
if exist "%~dp0images\super.img" (
    echo Removing existing super.img
    del "%~dp0images\super.img" || (
        echo "Failed to delete existing super.img"
        exit /B 1
    )
)

REM Decompress the super.img.zst file to the super.img file
echo Decompressing super.img.zst to super.img
zstd -d "%~dp0/super.img.zst" -o "%~dp0images/super.img" || (
    echo "Decompression error"
    exit /B 1
)

REM Flash super.img
echo Flashing super partition
fastboot %* flash super "%~dp0images/super.img" || (
    echo "Flash super error"
    exit /B 1
)

REM Erase boot_ab partition
echo Erasing boot_ab partition
fastboot %* erase boot_ab || (
    echo "Erase boot error"
    exit /B 1
)

REM Flash individual partitions
echo Flashing abl_ab
fastboot %* flash abl_ab "%~dp0images/abl.img" || (
    echo "Flash abl_ab error"
    exit /B 1
)

echo Flashing xbl_ab
fastboot %* flash xbl_ab "%~dp0images/xbl.img" || (
    echo "Flash xbl_ab error"
    exit /B 1
)

echo Flashing xbl_config_ab
fastboot %* flash xbl_config_ab "%~dp0images/xbl_config.img" || (
    echo "Flash xbl_config_ab error"
    exit /B 1
)

echo Flashing shrm_ab
fastboot %* flash shrm_ab "%~dp0images/shrm.img" || (
    echo "Flash shrm_ab error"
    exit /B 1
)

echo Flashing aop_ab
fastboot %* flash aop_ab "%~dp0images/aop.img" || (
    echo "Flash aop_ab error"
    exit /B 1
)

echo Flashing aop_config_ab
fastboot %* flash aop_config_ab "%~dp0images/aop_config.img" || (
    echo "Flash aop_config_ab error"
    exit /B 1
)

echo Flashing tz_ab
fastboot %* flash tz_ab "%~dp0images/tz.img" || (
    echo "Flash tz_ab error"
    exit /B 1
)

echo Flashing devcfg_ab
fastboot %* flash devcfg_ab "%~dp0images/devcfg.img" || (
    echo "Flash devcfg_ab error"
    exit /B 1
)

echo Flashing featenabler_ab
fastboot %* flash featenabler_ab "%~dp0images/featenabler.img" || (
    echo "Flash featenabler_ab error"
    exit /B 1
)

echo Flashing hyp_ab
fastboot %* flash hyp_ab "%~dp0images/hyp.img" || (
    echo "Flash hyp_ab error"
    exit /B 1
)

echo Flashing uefi_ab
fastboot %* flash uefi_ab "%~dp0images/uefi.img" || (
    echo "Flash uefi_ab error"
    exit /B 1
)

echo Flashing uefisecapp_ab
fastboot %* flash uefisecapp_ab "%~dp0images/uefisecapp.img" || (
    echo "Flash uefisecapp_ab error"
    exit /B 1
)

echo Flashing modem_ab
fastboot %* flash modem_ab "%~dp0images/modem.img" || (
    echo "Flash modem_ab error"
    exit /B 1
)

echo Flashing bluetooth_ab
fastboot %* flash bluetooth_ab "%~dp0images/bluetooth.img" || (
    echo "Flash bluetooth_ab error"
    exit /B 1
)

echo Flashing dsp_ab
fastboot %* flash dsp_ab "%~dp0images/dsp.img" || (
    echo "Flash dsp_ab error"
    exit /B 1
)

echo Flashing keymaster_ab
fastboot %* flash keymaster_ab "%~dp0images/keymaster.img" || (
    echo "Flash keymaster_ab error"
    exit /B 1
)

echo Flashing qupfw_ab
fastboot %* flash qupfw_ab "%~dp0images/qupfw.img" || (
    echo "Flash qupfw_ab error"
    exit /B 1
)

echo Flashing cpucp_ab
fastboot %* flash cpucp_ab "%~dp0images/cpucp.img" || (
    echo "Flash cpucp_ab error"
    exit /B 1
)

echo Flashing rescue
fastboot %* flash rescue "%~dp0images/rescue.img" || (
    echo "Flash rescue error"
    exit /B 1
)

echo Flashing xbl_ramdump_ab
fastboot %* flash xbl_ramdump_ab "%~dp0images/xbl_ramdump.img" || (
    echo "Flash xbl_ramdump_ab error"
    exit /B 1
)

echo Flashing imagefv_ab
fastboot %* flash imagefv_ab "%~dp0images/imagefv.img" || (
    echo "Flash imagefv_ab error"
    exit /B 1
)

echo Flashing vendor_boot_ab
fastboot %* flash vendor_boot_ab "%~dp0images/vendor_boot.img" || (
    echo "Flash vendor_boot_ab error"
    exit /B 1
)

echo Flashing dtbo_ab
fastboot %* flash dtbo_ab "%~dp0images/dtbo.img" || (
    echo "Flash dtbo_ab error"
    exit /B 1
)

echo Flashing vbmeta_ab
fastboot %* flash vbmeta_ab "%~dp0images/vbmeta.img" || (
    echo "Flash vbmeta_ab error"
    exit /B 1
)

echo Flashing vbmeta_system_ab
fastboot %* flash vbmeta_system_ab "%~dp0images/vbmeta_system.img" || (
    echo "Flash vbmeta_system_ab error"
    exit /B 1
)

echo Flashing cust
fastboot %* flash cust "%~dp0images/cust.img" || (
    echo "Flash cust error"
    exit /B 1
)

echo Flashing recovery_ab
fastboot %* flash recovery_ab "%~dp0images/recovery.img" || (
    echo "Flash recovery_ab error"
    exit /B 1
)

echo Flashing boot_ab
fastboot %* flash boot_ab "%~dp0images/boot.img" || (
    echo "Flash boot_ab error"
    exit /B 1
)

REM Erase and flash imagefv_ab partition
echo Erasing imagefv_ab partition
fastboot %* erase imagefv_ab || (
    echo "Erase imagefv error"
    exit /B 1
)
echo Flashing imagefv_ab partition
fastboot %* flash imagefv_ab "%~dp0images/imagefv.img" || (
    echo "Flash imagefv error"
    exit /B 1
)

REM Flash misc partition
echo Flashing misc partition
fastboot %* flash misc "%~dp0images/misc.img" || (
    echo "Flash misc error"
    exit /B 1
)

REM Flash vbmeta.img and vbmeta_system.img with verity and verification disabled
echo Flashing vbmeta partition with verity and verification disabled
fastboot %* --disable-verity --disable-verification flash vbmeta "%~dp0images/vbmeta.img" || (
    echo "Flash vbmeta error"
    exit /B 1
)
echo Flashing vbmeta_system partition with verity and verification disabled
fastboot %* --disable-verity --disable-verification flash vbmeta_system "%~dp0images/vbmeta_system.img" || (
    echo "Flash vbmeta_system error"
    exit /B 1
)

REM Set active partition to a
echo Setting active partition to a
fastboot %* set_active a || (
    echo "Set active a error"
    exit /B 1
)

REM Remove the decompressed super.img file if exists
if exist "%~dp0images\super.img" (
    echo Removing decompressed super.img file
    del "%~dp0images\super.img" || (
        echo "Failed to delete super.img"
        exit /B 1
    )
)

REM Reboot device
echo Rebooting device
fastboot %* reboot || (
    echo "Reboot error"
    exit /B 1
)

endlocal
exit 0
```

#### Card Flashing Script Example

```shell
#!/sbin/sh

set -e
ZIP="$3"
ZIPNAME="${ZIP##*/}"
OUTFD="/proc/self/fd/$2"

# Clean up /tmp directory and create a new one
[ -d /tmp ] && rm -rf /tmp
mkdir -p /tmp

# Unzip zstd executable to /tmp directory
unzip "$ZIP" bin/zstd -d /tmp
chmod -R 0755 /tmp

ui_print(){
    echo -e "ui_print $1\nui_print" > "$OUTFD"
}

package_extract_file(){
    if unzip -l "$ZIP" "$1" >/dev/null 2>&1; then
        unzip -p "$ZIP" "$1" > "$2"
    else
        ui_print "Error: Cannot find $1 in $ZIP"
        exit 1
    fi
}

package_extract_zstd() {
    if unzip -l "$ZIP" "$1" >/dev/null 2>&1; then
        unzip -p "$ZIP" "$1" | /tmp/bin/zstd -k -d > "$2"
    else
        ui_print "Error: Cannot find $1 in $ZIP"
        exit 1
    fi
}

PartitionFilePath="/dev/block/by-name"

flash_partition() {
    local partition="$1"
    ui_print "Flashing $partition partition"
    package_extract_file "images/$partition.img" "${PartitionFilePath}/${partition}_a"
    package_extract_file "images/$partition.img" "${PartitionFilePath}/${partition}_b"
}

# Output script information
ui_print "$ZIPNAME"
ui_print "Produced by Ranshen."
ui_print " "
ui_print " "

# Flash each partition
for partition in abl xbl xbl_config shrm aop aop_config tz devcfg featenabler hyp uefi uefisecapp modem bluetooth dsp keymaster qupfw cpucp xbl_ramdump imagefv vendor_boot dtbo vbmeta vbmeta_system recovery; do
    flash_partition "$partition"
done

ui_print "Flashing rescue partition"
package_extract_file "images/rescue.img" "${PartitionFilePath}/rescue"

ui_print "Flashing super partition"
package_extract_zstd "super.img.zst" "${PartitionFilePath}/super"

ui_print "Flashing cust partition"
package_extract_file "images/cust.img" "${PartitionFilePath}/cust"

ui_print "Flashing misc partition"
package_extract_file "images/misc.img" "${PartitionFilePath}/misc"

# Clear cache
ui_print "Clearing cache."
rm -rf /data/cache/
rm -rf /data/dalvik-cache/
rm -rf /data/system/package_cache/

# Clear temporary directory
ui_print "Clearing /tmp directory."
rm -rf /tmp

ui_print " "
ui_print "Flashing completed."

exit 0
```