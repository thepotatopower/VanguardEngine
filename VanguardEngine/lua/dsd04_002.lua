-- 樹角獣 ラティス

function RegisterAbilities()
	-- on ride
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnRide)
	ability1.SetTrigger("OnRideTrigger")
	ability1.SetCost("OnRideCost")
	ability1.SetActivation("OnRide")
	-- on attack
	local ability2 = NewAbility(GetID())
	ability2.SetDescription(2)
	ability2.SetTiming(a.OnAttack)
	ability2.SetLocation(l.BackRowRC)
	ability2.SetTrigger("OnAttackTrigger")
	ability2.SetCost("OnAttackCost")
	ability2.SetCanFullyResolve("OnAttackCanFullyResolve")
	ability2.SetActivation("OnAttack")
end

function OnRideTrigger()
	return obj.WasRodeUponBy(obj.GetNameFromCardID("dsd04_001"))
end

function OnRideCost(check)
	if check then return obj.CanSB(1) end
	obj.SoulBlast(1)
end

function OnRide()
	obj.RevealFromDeck(1)
	obj.SuperiorCall({q.Location, l.Revealed, q.Other, o.Unit})
	obj.AddToHand({q.Location, l.Revealed})
end

function OnAttackTrigger()
	return obj.IsAttackingUnit() and obj.Exists({q.Location, l.EnemyVC, q.Other, o.Attacked})
end

function OnAttackCost(check)
	if check then return obj.CanSB(1) end
	obj.SoulBlast(1)
end

function OnAttackCanFullyResolve()
	return obj.IsSameZone()
end

function OnAttack()
	obj.AddCardValue({q.Location, l.PlayerUnits, q.Other, o.ThisFieldID}, cs.BonusPower, 10000, p.UntilEndOfBattle)
end