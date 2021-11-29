-- グラビディア・オルゲイユ

function RegisterAbilities()
	-- on attack
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnAttack)
	ability1.SetLocation(l.RC)
	ability1.SetTrigger("Trigger")
	ability1.SetCondition("Condition")
	ability1.SetCost("Cost")
	ability1.SetCanFullyResolve("CanFullyResolve")
	ability1.SetActivation("Activation")
end

function Trigger()
	return obj.IsAttackingUnit() or obj.IsBooster()
end

function Condition()
	return obj.GetNumberOf({q.Location, l.Order, q.Other, o.Meteorite}) >= 3 
end

function Cost(check)
	if check then return obj.CanSB(2) end
	obj.SoulBlast(2)
end

function CanFullyResolve()
	return obj.Exists({q.Location, l.Damage, q.Other, o.FaceDown}) or obj.IsSameZone()
end

function Activation()
	obj.CounterCharge(1)
	if obj.IsSameZone() then
		obj.AddCardValue({q.Location, l.PlayerUnits, q.Other, o.This}, cs.BonusPower, 5000, p.UntilEndOfTurn)
	end
end