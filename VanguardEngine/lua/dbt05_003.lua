-- ファントム・ブラスター・オーバーロード

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
	ability2.SetCost("Cost")
	ability2.SetCanFullyResolve("CanFullyResolve")
	ability2.SetActivation("Activation")
end

function Cont()
	if obj.Exists({q.Location, l.Soul, q.Name, obj.GetNameFromCardID("dbt02_008")}) then
		obj.AddCardValue({q.Other, o.This}, cs.BonusCritical, 1, p.Continuous)
	end
end

function Trigger()
	return obj.IsAttackingUnit()
end

function Cost(check)
	if check then return obj.CanRetire({q.Location, l.PlayerRC, q.Count, 2}) end
	obj.ChooseRetire({q.Location, l.PlayerRC, q.Count, 2})
end

function CanFullyResolve()
	return obj.CanSuperiorCall({q.Location, l.Soul, q.Location, l.Drop, q.Name, obj.GetNameFromCardID("dbt02_046"), q.Count, 1})
end

function Activation()
	obj.Store(obj.SuperiorCall({q.Location, l.Soul, q.Location, l.Drop, q.Name, obj.GetNameFromCardID("dbt02_046"), q.Count, 1}))
	if obj.Exists({q.Location, l.EnemyVC, q.Grade, 3, q.Other, o.GradeOrHigher}) then
		obj.AddCardValue({q.Other, o.ThisFieldID}, cs.BonusPower, 10000, p.UntilEndOfTurn)
		obj.AddCardValue({q.Location, l.Stored}, cs.BonusPower, 10000, p.UntilEndOfTurn)
	end
end