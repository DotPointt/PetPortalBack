#!/bin/bash

echo "=== Инициализация SSL сертификатов ==="

# Создаем необходимые директории
mkdir -p ./data/certbot/conf
mkdir -p ./data/certbot/www
mkdir -p ./dist

# Копируем временную конфигурацию nginx
cp nginx-init.conf PetPortalBack/nginx.conf

# Запускаем nginx с упрощенной конфигурацией
echo "Запуск nginx для верификации домена..."
docker compose up -d nginx

# Ждем запуска nginx
echo "Ожидание запуска nginx..."
sleep 5

# Проверяем, что nginx работает
if ! curl -f http://localhost/.well-known/acme-challenge/test 2>/dev/null; then
    echo "Проверка доступности nginx..."
    sleep 10
fi

# Получаем SSL сертификат
echo "Получение SSL сертификата от Let's Encrypt..."
docker compose run --rm --entrypoint "\
  certbot certonly --webroot \
  --webroot-path=/var/www/certbot \
  --register-unsafely-without-email \
  --agree-tos \
  -d pet-projects.online \
  -d www.pet-projects.online \
  --dry-run" certbot

# Если dry-run успешен, получаем реальный сертификат
if [ $? -eq 0 ]; then
    echo "Dry-run успешен. Получаем реальный сертификат..."
    docker compose run --rm --entrypoint "\
      certbot certonly --webroot \
      --webroot-path=/var/www/certbot \
      --register-unsafely-without-email \
      --agree-tos \
      -d pet-projects.online \
      -d www.pet-projects.online" certbot
else
    echo "Ошибка при dry-run. Проверьте настройки."
    exit 1
fi

# Копируем основную конфигурацию nginx обратно
cp nginx-main.conf PetPortalBack/nginx.conf

# Перезапускаем nginx с SSL
echo "Перезапуск nginx с SSL конфигурацией..."
docker compose restart nginx

# Запускаем остальные сервисы
echo "Запуск всех сервисов..."
docker compose up -d

echo "=== Инициализация завершена! ==="
echo "Проверьте сертификат:"
docker compose exec nginx nginx -t
echo ""
echo "Сервис должен быть доступен по:"
echo "  https://pet-projects.online"
echo ""
echo "Проверьте логи nginx: docker compose logs nginx"