# Servicio Social
# Actualizar el índice de git según el nuevo .gitignore
git rm -r --cached .vs bin obj applicationhost.config

# Confirmar la limpieza
git commit -m "Agregado .gitignore y limpieza de archivos locales/temporales"

git push origin master
