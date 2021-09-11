-- Regurgitation from the Underworld

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 3
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.PlayerRC, q.Location, l.EnemyRC, q.Other, o.CanChoose, q.Count, 1
	elseif n == 2 then
		return q.Location, l.Drop, q.Grade, 0, q.Count, 0, q.Min, 0
	elseif n == 3 then
		return q.Location, l.Selected, q.Count, 1
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnOrder, t.Order, p.HasPrompt, p.CB, 1, p.SB, 1
	end
end

function CheckCondition(n)
	if n == 1 then
		return true
	end
	return false
end

function CanFullyResolve(n)
	if n == 1 then
		if obj.Exists(1) then
			return true
		end
	end
	return false
end

function Activate(n)
	if n == 1 then
		obj.Select(1)
		if obj.Exists(3) then
			obj.Inject(2, q.Grade, obj.GetSelectedGrade(3))
			obj.Retire(3)
			if obj.IsAlchemagic() then
				obj.Inject(2, q.Count, 2)
			else
				obj.Inject(2, q.Count, 1)
			end
			obj.SuperiorCall(2)
		end
		obj.EndSelect()
	end
	return 0
end