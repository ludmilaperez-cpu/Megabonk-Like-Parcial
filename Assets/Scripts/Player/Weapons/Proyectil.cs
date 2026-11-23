using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Proyectil : BulletController
{
    [SerializeField] float _angle;

    protected override void Start()
    {
        base.Start();
        //transform.rotation = new Quaternion (transform.rotation.x, transform.rotation.y, transform.rotation.z, transform.rotation.w);
        //transform.rotation = new Quaternion(_angle, transform.rotation.y, transform.rotation.z, transform.rotation.w);
    }

    protected override void Shoot()
    {
        if (_target == null) return;

        Vector3 relativePos = _target.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.time * _speed);

        transform.position += transform.rotation * transform.forward *Time.deltaTime;
        transform.position += transform.rotation * _direction.normalized * _speed * Time.deltaTime;
    }
}
