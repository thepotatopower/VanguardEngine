-- Spiritual Body Condensation

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 3
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.Drop, q.Grade, 0, q.Other, o.GradeOrLess, q.Count, 1
	elseif n == 2 then
		return q.Location, l.Drop, q.Count, 1
	elseif n == 3 then
		return q.Location, l.Selected
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnOrder, p.HasPrompt, p.SB, 1
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
		if obj.Exists(2) then
			return true
		end
	end
	return false
end

function Activate(n)
	if n == 1 then
		obj.Inject(1, q.Grade, obj.VanguardGrade())
		if obj.Exists(1) then
			obj.Select(1)
			obj.SuperiorCall(3)
			obj.AddTempPower(3, 5000)
			obj.EndSelect()
		end
	end
	return 0
end