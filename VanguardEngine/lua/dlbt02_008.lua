-- みゃーみゃーあんさんぶる ナラ

function RegisterAbilities()
	-- on place
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.PlacedOnRC)
	ability1.SetTrigger("OnPlaceTrigger")
	ability1.SetCost("OnPlaceCost")
	ability1.SetCanFullyResolve("OnPlaceCanFullyResolve")
	ability1.SetActivation("OnPlace")
	-- on attack
	local ability2 = NewAbility(GetID())
	ability2.SetDescription(2)
	ability2.SetProperty(p.Powerful)
	ability2.SetTiming(a.OnAttack)
	ability2.SetLocation(l.RC)
	ability2.SetTrigger("OnAttackTrigger")
	ability2.SetCondition("OnAttackCondition")
	ability2.SetActivation("OnAttack")
end

function OnPlaceTrigger()
	return obj.IsApplicable() and obj.PlayerMainPhase()
end

function OnPlaceCost(check)
	if check then return obj.CanCB(1) end
	obj.CounterBlast(1)
end

function OnPlaceCanFullyResolve()
	return obj.CanSuperiorCall("", {l.Soul}, FL.OpenCircle)
end

function OnPlace()
	obj.SuperiorCall("", {l.Soul}, 1, 1, true, "", false, {FL.OpenCircle})
	obj.Discard(obj.SoulCount());
end

function OnAttackTrigger()
	return obj.IsApplicable() and obj.Exists({q.Location, l.EnemyVC, q.Grade, 3, q.Other, o.GradeOrHigher})
end

function OnAttackCondition()
	return obj.SoulCount() == 0 and obj.IsSameZone()
end

function OnAttack()
	obj.AddCardValue({q.Other, o.ThisFieldID}, cs.BonusPower, 5000, p.UntilEndOfTurn)
end