using Projekat3;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Projekat2
{
    internal class Cache
    {
        private ReaderWriterLockSlim _cacheLock;
        private Dictionary<string, Repository> _kes;
        private int interval = 30000;
        private bool _istekao = false;

        public Cache()
        {
            _cacheLock = new ReaderWriterLockSlim();
            _kes = new Dictionary<string, Repository>();
            PokreniPeriodicnoBrisanje();
        }

        private async void PokreniPeriodicnoBrisanje()
        {
            while (true)
            {
                await Task.Delay(interval);
                _istekao = true;
                PeriodicnoBrisanje();
            }
        }

        public void DodajUKes(string key, Repository s)
        {
            _cacheLock.EnterReadLock();
            try
            {
                if (_kes.ContainsKey(key))
                {
                    _kes[key].CreatedOnTime = DateTime.Now;
                    return;
                }
                _kes.Add(key, s);
                //TrenutnoStanje();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                _cacheLock.ExitReadLock();
            }
        }

        public void TrenutnoStanje()
        {
            Console.WriteLine("Kljucevi koji se nalaze u kesu su:");
            foreach (var key in _kes.Keys)
            {
                Console.WriteLine($" {key}  ");
            }
            Console.WriteLine("\n");
        }

        public void StampajStavkuKesa(string key)
        {
            _cacheLock.EnterReadLock();
            try
            {
                if (_kes.TryGetValue(key, out Repository k))
                {
                    Console.WriteLine($"Kljuc je: {key}, podaci su: {k.ToString()} \n");
                }
                else
                {
                    Console.WriteLine("Kljuc nije pronadjen.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                _cacheLock.ExitReadLock();
            }
        }

        public void ObrisiCeoKes()
        {
            _cacheLock.EnterReadLock();
            try
            {
                _kes.Clear();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                _cacheLock.ExitReadLock();
            }
        }

        public void PeriodicnoBrisanje()
        {
            if (_istekao)
            {
                _cacheLock.EnterReadLock();
                try
                {
                    DateTime trenutnoVreme = DateTime.Now;
                    List<string> istekliKljucevi = new List<string>();

                    foreach (var k in _kes)
                    {
                        if (trenutnoVreme.Subtract(k.Value.CreatedOnTime).TotalMilliseconds >= interval)
                        {
                            istekliKljucevi.Add(k.Key);
                        }
                    }

                    foreach (string key in istekliKljucevi)
                    {
                        _kes.Remove(key);
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    _istekao = false;
                    _cacheLock.ExitReadLock();
                }
            }
        }

        public void ObrisiIzKesa(string key)
        {
            _cacheLock.EnterReadLock();
            try
            {
                if (_kes.Remove(key))
                    Console.WriteLine("Obrisan key: %s \n", key);
                else
                    Console.WriteLine("Ne postoji key: %s \n", key);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                _cacheLock.ExitReadLock();
            }
        }

        public Repository CitajIzKesa(string key)
        {
            _cacheLock.EnterReadLock();
            try
            {
                if (_kes.TryGetValue(key, out Repository stavka))
                    return stavka;
                else
                    return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
            finally
            {
                _cacheLock.ExitReadLock();
            }
        }

        public bool ImaKljuc(string key)
        {
            _cacheLock.EnterReadLock();
            try
            {
                return _kes.ContainsKey(key);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            finally
            {
                _cacheLock.ExitReadLock();
            }
        }
    }
}