-- Regurgitation from the Underworld

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 5
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.Damage, q.Count, 1
	elseif n == 2 then
		return q.Location, l.Soul, q.Count, 1
	elseif n == 3 then
		return q.Location, l.PlayerRC, q.Location, l.EnemyRC, q.Other, o.CanChoose, q.Count, 1
	elseif n == 4 then
		return q.Location, l.Drop, q.Grade, 0, q.Count, 0, q.Min, 0
	elseif n == 5 then
		return q.Location, l.Selected, q.Count, 1
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnOrder, t.Order, p.HasPrompt, true, p.IsMandatory, false, p.CB, 1, p.SB, 1
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.CanCB(1) and obj.CanSB(2) then
			return true
		end
	end
	return false
end

function CanFullyResolve(n)
	if n == 1 then
		if obj.Exists(3) then
			return true
		end
	end
	return false
end

function Cost(n)
	if n == 1 then
		obj.CounterBlast(1)
		obj.SoulBlast(2)
	end
end

function Activate(n)
	if n == 1 then
		obj.Select(3)
		if obj.Exists(5) then
			obj.Retire(5)
			obj.Inject(4, q.Grade, obj.GetSelectedGrade(5))
			if obj.IsAlchemagic() then
				obj.Inject(4, q.Count, 2)
			else
				obj.Inject(4, q.Count, 1)
			end
			obj.SuperiorCall(4)
		end
		obj.EndSelect()
	end
	return 0
end