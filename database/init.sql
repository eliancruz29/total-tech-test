-- ========================================
-- Database Initialization Script
-- Sistema de Adquisiciones CAASIM
-- ========================================

\echo '========================================';
\echo 'Initializing Adquisiciones CAASIM Database';
\echo '========================================';

-- Set connection encoding
SET client_encoding = 'UTF8';

\echo 'Running Migration 001: Create Catalog Tables...';
\i /docker-entrypoint-initdb.d/migrations/001_create_catalog_tables.sql

\echo 'Running Migration 002: Create Main Process Tables...';
\i /docker-entrypoint-initdb.d/migrations/002_create_main_tables.sql

\echo 'Running Migration 003: Seed Initial Data...';
\i /docker-entrypoint-initdb.d/migrations/003_seed_data.sql

\echo '========================================';
\echo 'Database Initialization Complete!';
\echo '========================================';
