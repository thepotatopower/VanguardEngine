-- ヴェルリーナ・エスペラルイデア

function RegisterAbilities()
	-- overDress requirement
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetOverDress("IsOverDressTarget")
	-- on place
	local ability2 = NewAbility(GetID())
	ability2.SetDescription(2)
	ability2.SetTiming(a.PlacedOnRC)
	ability2.SetTrigger("OnPlaceTrigger")
	ability2.SetCondition("OnPlaceCondition")
	ability2.SetActivation("OnPlace")
	-- on attack
	local ability3 = NewAbility(GetID())
	ability3.SetDescription(3)
	ability3.SetTiming(a.OnAttack)
	ability3.SetLocation(l.RC)
	ability3.SetProperty(p.OncePerTurn)
	ability3.SetTrigger("OnAttackTrigger")
	ability3.SetCost("OnAttackCost")
	ability3.SetCanFullyResolve("OnAttackCanFullyResolve")
	ability3.SetActivation("OnAttack")
end

function IsOverDressTarget(id)
	obj.Store(id)
	return obj.Exists({q.Location, l.Stored, q.Name, obj.GetNameFromCardID("dsd01_009")})
end

function OnPlaceTrigger()
	return obj.IsApplicable()
end

function OnPlaceCondition()
	return obj.Exists({q.Location, l.Applicable, q.Other, o.ThisFieldID, q.Other, o.OverDress}) and 
	(obj.Exists({q.Location, l.Drop, q.Name, obj.GetNameFromCardID("dsd01_006")}) or obj.Exists({q.Location, l.Drop, q.UnitType, u.overDress}))
end

function OnPlace()
	local group = obj.AddGroups({q.Location, l.Drop, q.Name, obj.GetNameFromCardID("dsd01_009")}, {q.Location, l.Drop, q.UnitType, u.overDress})
	obj.Store(group)
	obj.AddToOriginalDress({q.Location, l.Applicable}, {q.Location, l.Stored})
end

function OnAttackTrigger()
	return obj.Exists({q.Location, l.PlayerVC, q.Other, o.Attacking})
end

function OnAttackCanFullyResolve()
	return obj.IsSameZone()
end

function OnAttackCost(check)
	if check then return obj.CanSB(1) end
	obj.SoulBlast(1)
end

function OnAttack()
	obj.Stand({q.Other, o.ThisFieldID})
	obj.AddCardValue({q.Other, o.ThisFieldID}, cs.BonusPower, 5000 * obj.GetNumberOf({q.Location, l.MyOriginalDress}), p.UntilEndOfTurn)
end

