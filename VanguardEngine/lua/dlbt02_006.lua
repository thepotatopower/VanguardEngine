-- 秘めたる宝 フェナエル

function RegisterAbilities()
	-- on place
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.PlacedOnRC)
	ability1.SetTrigger("OnPlaceTrigger")
	ability1.SetCondition("OnPlaceCondition")
	ability1.SetCost("OnPlaceCost")
	ability1.SetCanFullyResolve("OnPlaceCanFullyResolve")
	ability1.SetActivation("OnPlace")
	-- on attack
	local ability2 = NewAbility(GetID())
	ability2.SetDescription(2)
	ability2.SetProperty(p.BlackWings)
	ability2.SetTiming(a.OnAttack)
	ability2.SetTrigger("OnAttackTrigger")
	ability2.SetCost("OnAttackCost")
	ability2.SetCanFullyResolve("OnAttackCanFullyResolve")
	ability2.SetActivation("OnAttack")
end

function OnPlaceTrigger()
	return obj.IsApplicable()
end

function OnPlaceCondition()
	return obj.GetNumberOf("AlestielVC", {l.Units}) > 0
end

function AlestielVC(id)
	return obj.IsPlayer(id) and obj.IsVanguard(id) and obj.NameContains(id, obj.LoadName("Alestiel"))
end

function OnPlaceCost(check)
	if check then return obj.CanSB(1) end
	obj.SoulBlast(1)
end

function OnPlaceCanFullyResolve()
	return obj.GetNumberOf("", {l.Bind}) > 0
end

function OnPlace()
	obj.Select("", {l.Bind}, 1, 1, {}, Prompt.AddToHand)
	obj.Store(obj.AddToHand({q.Location, l.Selected}))
	if obj.GetNumberOf({q.Location, l.Stored}) > 0 then
		obj.Select("", {l.Hand}, 1, 1, {}, Prompt.Bind)
		obj.Bind({q.Location, l.Selected})
	end
end

function OnAttackTrigger()
	return obj.IsApplicable() and obj.GetNumberOf("VanguardIsAttacked", {l.Units}) > 0
end

function VanguardIsAttacked(id)
	return not obj.IsPlayer(id) and obj.IsVanguard(id)
end

function OnAttackCost(check)
	if check then return obj.CanCB(1) end
	obj.CounterBlast(1)
end

function OnAttackCanFullyResolve()
	return obj.GetNumberOf("OnAttackFilter", {l.Units}) > 0
end

function OnAttackFilter(id)
	return not obj.IsPlayer(id) and obj.GradeOf(id) >= 3 and obj.IsVanguard(id) and obj.CanChoose(id)
end

function OnAttack()
	obj.Select("OnAttackFilter", {l.Units}, 1, 1, {}, -1)
	obj.AddCardValue({q.Location, l.Selected}, cs.BonusPower, -5000, p.UntilEndOfTurn)
end