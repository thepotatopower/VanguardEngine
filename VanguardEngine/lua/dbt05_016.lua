-- 冷徹な遂行者 ミカニ

function RegisterAbilities()
	-- on place
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.PlacedOnRC)
	ability1.SetLocation(l.RC)
	ability1.SetTrigger("PlacedOnRCTrigger")
	ability1.SetCost("PlacedOnRCCost")
	ability1.SetActivation("PlacedOnRC")
	-- on attack
	local ability2 = NewAbility(GetID())
	ability2.SetDescription(2)
	ability2.SetProperty(p.Glitter)
	ability2.SetTiming(a.OnAttack)
	ability2.SetLocation(l.RC)
	ability2.SetProperty(p.OncePerTurn)
	ability2.SetTrigger("OnAttackTrigger")
	ability2.SetCondition("OnAttackCondition")
	ability2.SetActivation("OnAttack")
end

function PlacedOnRCTrigger()
	return obj.IsApplicable()
end

function PlacedOnRCCost(check)
	if check then return obj.CanCB(1) and obj.CanSB(1) end
	obj.CounterBlast(1)
	obj.Store(obj.SoulBlast(1))
end

function Filter(id)
	return not obj.IsPlayer(id) and obj.IsRearguard(id) and obj.CanChoose(id)
end

function PlacedOnRC()
	obj.Select("Filter", {l.Units}, 1, 1, {}, -1)
	obj.Retire({q.Location, l.Selected})
end

function OnAttackTrigger()
	return obj.IsAttackingUnit()
end

function OnAttackCondition()
	return obj.IsSameZone() and obj.GetNumberOf("GlitterVG", {l.Units}) > 0 and obj.GetNumberOf({q.Location, l.Soul, q.Other, o.DifferentNames}) >= 8
end

function GlitterVG(id)
	return obj.IsPlayer(id) and obj.IsVanguard(id) and obj.NameContains(id, obj.LoadName("Kheios")) and obj.HasProperty(id, p.IsGlitter)
end

function OnAttack()
	obj.Stand({q.Other, o.ThisFieldID})
end
