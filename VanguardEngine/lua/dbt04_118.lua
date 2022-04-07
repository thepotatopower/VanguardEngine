-- 叫喚は雨音に融けよ

function RegisterAbilities()
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnOrder)
	ability1.SetCanFullyResolve("CanFullyResolve")
	ability1.SetActivation("Activation")
end

function CanFullyResolve()
	return obj.Exists({q.Location, l.EnemyRC, q.Other, o.CanChoose, q.Count, 1})
end

function Activation()
	obj.EnemyChooseRetire({q.Location, l.EnemyRC, q.Other, o.CanChoose, q.Count, 1})
	local grade = obj.GetGrade({q.Location, l.SuccessfullyRetired})
	obj.SuperiorCall({q.Location, l.Drop, q.Grade, grade, q.Count, 1})
end