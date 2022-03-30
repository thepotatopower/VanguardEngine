-- マジェスティ・ロードブラスター

function RegisterAbilities()
	-- cont
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.Cont)
	ability1.SetLocation(l.VC)
	ability1.SetActivation("Cont")
	-- on attack
	local ability2 = NewAbility(GetID())
	ability2.SetDescription(2)
	ability2.SetTiming(a.OnAttack)
	ability2.SetLocation(l.VC)
	ability2.SetTrigger("Trigger")
	ability2.SetCondition("Condition")
	ability2.SetActivation("Activation")
end

function Cont()
	if obj.Exists({q.Location, l.Soul, q.Name, obj.GetNameFromCardID("dbt05_005")}) and obj.Exists({q.Location, l.Soul, q.Name, obj.GetNameFromCardID("dbt02_046")}) then
		obj.AddCardValue({q.Other, o.This}, cs.BonusPower, 2000, p.Continuous)
		obj.AddCardValue({q.Other, o.This}, cs.BonusCritical, 1, p.Continuous)
	end
end

function Trigger()
	return obj.IsAttackingUnit() and obj.Exists({q.Location, l.EnemyVC, q.Other, o.Attacked})
end

function Condition()
	return CanFullyResolve1() or CanFullyResolve2()
end

function Activation()
	local option = obj.SelectOption({"Cost1", "CanFullyResolve1", obj.GetDescription(3)}, {"Cost2", "CanFullyResolve2", obj.GetDescription(4)}, {"SKIP"})
	if option == 1 then
		obj.PayCost("Cost1")
		obj.ChooseRetire({q.Location, l.EnemyRC, q.Other, o.CanChoose, q.Count, 1})
	elseif option == 2 then
		obj.PayCost("Cost2")
		obj.AddCardValue({q.Other, o.ThisFieldID}, cs.BonusDrive, 1, p.UntilEndOfTurn)
	end
	if option == 1 and obj.CanPayCost("Cost2") then
		option = obj.SelectOption({"Cost2", "CanFullyResolve2", obj.GetDescription(4)}, {"SKIP"})
		if option == 1 then
			obj.PayCost("Cost2")
			obj.AddCardValue({q.Other, o.ThisFieldID}, cs.BonusDrive, 1, p.UntilEndOfTurn)
		end
	elseif option == 2 and obj.CanPayCost("Cost1") then
		option = obj.SelectOption({"Cost1", "CanFullyResolve1", obj.GetDescription(3)}, {"SKIP"})
		if option == 1 then
			obj.PayCost("Cost1")
			obj.ChooseRetire({q.Location, l.EnemyRC, q.Other, o.CanChoose, q.Count, 1})
		end
	end
end

function Cost1(check)
	if check then return obj.CanAddToSoul({q.Location, l.PlayerRC, q.Name, obj.GetNameFromCardID("dbt05_005"), q.Count, 1}) end
	obj.ChooseAddToSoul({q.Location, l.PlayerRC, q.Name, obj.GetNameFromCardID("dbt05_005"), q.Count, 1})
end

function CanFullyResolve1()
	return obj.Exists({q.Location, l.EnemyRC, q.Other, o.CanChoose, q.Count, 1})
end

function Cost2(check)
	if check then return obj.CanAddToSoul({q.Location, l.PlayerRC, q.Name, obj.GetNameFromCardID("dbt02_046"), q.Count, 1}) end
	obj.ChooseAddToSoul({q.Location, l.PlayerRC, q.Name, obj.GetNameFromCardID("dbt02_046"), q.Count, 1})
end

function CanFullyResolve2()
	return obj.IsSameZone()
end