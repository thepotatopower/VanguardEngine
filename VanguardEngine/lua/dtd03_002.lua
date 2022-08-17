-- 烈光の騎士 ユース

function RegisterAbilities()
	-- on ride
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnRide)
	ability1.SetTrigger("OnRideTrigger")
	ability1.SetCost("OnRideCost")
	ability1.SetCanFullyResolve("OnRideCanFullyResolve")
	ability1.SetActivation("OnRide")
	-- on attack
	local ability2 = NewAbility(GetID())
	ability2.SetDescription(2)
	ability2.SetTiming(a.OnAttack)
	ability2.SetLocation(l.VC, l.RC)
	ability2.SetTrigger("OnAttackTrigger")
	ability2.SetCost("OnAttackCost")
	ability2.SetCanFullyResolve("OnAttackCanFullyResolve")
	ability2.SetActivation("OnAttack")
end

function OnRideTrigger()
	return obj.IsApplicable() and obj.GetNumberOf("Filter", {l.PlayerVC}) > 0
end

function Filter(id)
	return obj.HasProperty(id, p.RevolDress)
end

function OnRideCost(check)
	if check then return obj.CanCB(1) end
	obj.CounterBlast(1)
end

function OnRideCanFullyResolve()
	return obj.CanAddToHand({q.Location, l.Drop, q.Grade, 2, q.Other, o.GradeOrGreater})
end

function OnRide()
	obj.Select({q.Location, l.Drop, q.Grade, 2, q.Other, o.GradeOrGreater, q.Count, 1}, Prompt.AddToHand)
	obj.AddToHand({q.Location, l.Selected})
end

function OnAttackTrigger()
	return obj.IsBoosted()
end

function OnAttackCost(check)
	if check then return obj.CanSB(1) end
	obj.SoulBlast(1)
end

function OnAttackCanFullyResolve()
	return obj.IsSameZone()
end

function OnAttack()
	obj.AddCardValue({q.Other, o.ThisFieldID}, cs.BonusPower, 5000, p.UntilEndOfBattle)
end