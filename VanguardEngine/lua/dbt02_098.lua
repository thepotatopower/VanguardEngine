-- Heavenly Blade of Magnificence, Bestida

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 1
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.EnemyRC, q.Column, 0
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnDiscard, p.HasPrompt, p.CB, 2, p.SB, 1
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.LastDiscarded() then
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

function Activate(n)
	if n == 1 then
		local column = obj.SelectColumn()
		obj.Inject(1, q.Column, column) 
		if obj.GetNumberOf(1) > 0 then
			obj.EnemyRearrangeOnBottom(1)
		end
	end
	return 0
end