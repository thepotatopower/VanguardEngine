-- 柩機の竜 シュティルユーゲ

function RegisterAbilities()
	-- on attack
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnAttack)
	ability1.SetLocation(l.RC)
	ability1.SetTrigger("Trigger")
	ability1.SetCost("Cost")
	ability1.SetCanFullyResolve("CanFullyResolve")
	ability1.SetActivation("Activation")
end

function Trigger()
	return obj.IsAttackingUnit() and obj.Exists({q.Location, l.PlayerUnits, q.Name, obj.GetNameFromCardID("dbt01_t01"), q.UnitType, u.Token, q.Other, o.Boosting})
end

function Cost(check)
	if check then return obj.CanSB(1) end
	obj.SoulBlast(1)
end

function CanFullyResolve()
	return obj.Exists({q.Location, l.Damage, q.Other, o.FaceDown})
end

function Activation()
	obj.CounterCharge(1)
end