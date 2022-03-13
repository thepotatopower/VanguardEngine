-- ブラスター・ブレード

function RegisterAbilities()
	-- on place VC
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.PlacedOnVC)
	ability1.SetTrigger("PlacedOnVCTrigger")
	ability1.SetCost("Cost")
	ability1.SetActivation("PlacedOnVC")
	-- on place RC
	local ability2 = NewAbility(GetID())
	ability2.SetDescription(2)
	ability2.SetTiming(a.PlacedOnRC)
	ability2.SetTrigger("PlacedOnRCTrigger")
	ability2.SetCost("Cost")
	ability2.SetCanFullyResolve("PlacedOnRCCanFullyResolve")
	ability2.SetActivation("PlacedOnRC")
end

function PlacedOnVCTrigger()
	return obj.IsApplicable()
end

function Cost(check)
	if check then return obj.CanCB(1) end
	obj.CounterBlast(1)
end

function PlacedOnVC()
	obj.ChooseRetire({q.Location, l.EnemyRC, q.Other, o.CanChoose, q.Count, 1})
	if not obj.Exists({q.Location, l.SuccessfullyRetired}) then
		obj.Draw(1)
	end
end

function PlacedOnRCTrigger()
	return obj.IsApplicable()
end

function PlacedOnRCCanFullyResolve()
	return obj.CanRetire({q.Location, l.EnemyRC, q.Grade, 2, q.Other, o.CanChoose, q.Other, o.GradeOrHigher, q.Count, 1})
end

function PlacedOnRC()
	obj.ChooseRetire({q.Location, l.EnemyRC, q.Grade, 2, q.Other, o.CanChoose, q.Other, o.GradeOrHigher, q.Count, 1})
end