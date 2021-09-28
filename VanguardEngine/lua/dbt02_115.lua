-- Sylvan Horned Beast, Tealuf 

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 1
end

function GetParam(n)
	if n == 1 then
		return q.Other, o.This
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.Cont, p.IsMandatory
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
		return true
	end
	return false
end

function Activate(n)
	if n == 1 then
		obj.AddContinuousState(1, cs.CannotBeRidden)
		obj.AddContinuousState(1, cs.CannotBeCalledToFrontRow)
		obj.AddContinuousState(1, cs.CannotMoveToFrontRow)
	end
	return 0
end