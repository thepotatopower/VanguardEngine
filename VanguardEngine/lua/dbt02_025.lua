-- Wild Intelligence

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 2
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.Drop, q.Grade, 0, q.Other, o.GradeOrLess, q.Count, 0, q.Min, 0
	elseif n == 2 then
		return q.Location, l.Damage, q.Count, 1
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnOrder, t.Order, p.HasPrompt, true, p.IsMandatory, false, p.CB, 1
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.CanCB(2) then
			return true
		end
	end
	return false
end

function CanFullyResolve(n)
	if n == 1 then
		return true
	end
	return false
end

function Cost(n)
	if n == 1 then
		obj.CounterBlast(2)
	end
end

function Activate(n)
	if n == 1 then
		obj.Mill(3)
		obj.Inject(1, q.Grade, obj.VanguardGrade())
		if obj.VanguardIs("Sylvan Horned Beast King, Magnolia") then
			obj.Inject(1, q.Count, 2)
		else
			obj.Inject(1, q.Count, 1)
		end
		obj.SuperiorCall(1)
	end
	return 0
end