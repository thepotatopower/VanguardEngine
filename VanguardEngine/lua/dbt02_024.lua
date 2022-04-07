-- 逆流する冥府

function RegisterAbilities()
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnOrder)
	ability1.SetCost("Cost")
	ability1.SetGetCosts("GetCosts")
	ability1.SetActivation("Activation")
end

function Cost(check)
	if check then return obj.CanCB(1) and obj.CanSB(1) end
	obj.CounterBlast(1)
	obj.SoulBlast(1)
end

function GetCosts()
	return p.CB, 1, p.SB, 1
end

function Activation()
	obj.ChooseRetire({q.Location, l.PlayerRC, q.Location, l.EnemyRC, q.Other, o.CanChoose, q.Count, 1})
	if obj.IsAlchemagic() then
		obj.SuperiorCall({q.Location, l.Drop, q.Grade, obj.GetGrade({q.Location, l.SuccessfullyRetired}), q.Count, 2, q.Min, 0})
	else
		obj.SuperiorCall({q.Location, l.Drop, q.Grade, obj.GetGrade({q.Location, l.SuccessfullyRetired}), q.Count, 1, q.Min, 0})
	end
end