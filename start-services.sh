#!/bin/bash

# Renkler
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
NC='\033[0m' # No Color

# Proje dizini
PROJECT_DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"

echo -e "${BLUE}=== Stok Takip Servisleri Başlatılıyor ===${NC}\n"

# StokTakip.API'yi başlat
echo -e "${YELLOW}[1/2] StokTakip.API başlatılıyor...${NC}"
cd "$PROJECT_DIR/StokTakip.API"
dotnet run &
API_PID=$!
echo -e "${GREEN}StokTakip.API başlatıldı (PID: $API_PID)${NC}\n"

# Küçük bir gecikme
sleep 3

# StokTakip.Web'i başlat
echo -e "${YELLOW}[2/2] StokTakip.Web başlatılıyor...${NC}"
cd "$PROJECT_DIR/StokTakip.Web"
dotnet run &
WEB_PID=$!
echo -e "${GREEN}StokTakip.Web başlatıldı (PID: $WEB_PID)${NC}\n"

echo -e "${BLUE}=== Tüm Servisler Başarıyla Başlatıldı ===${NC}"
echo -e "${YELLOW}API PID: $API_PID${NC}"
echo -e "${YELLOW}Web PID: $WEB_PID${NC}"
echo -e "\n${YELLOW}Servisleri durdurmak için CTRL+C tuşlarına basın...${NC}\n"

# Ctrl+C sinyalini işle
trap 'kill $API_PID $WEB_PID 2>/dev/null; echo -e "\n${RED}=== Servisler Durduruldu ===${NC}"; exit 0' INT

# Processler sonlanana kadar bekle
wait

