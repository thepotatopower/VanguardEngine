-- Heavenly Blade of Magnificence, Bestida

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 3
end

function GetParam(n)
	if n == 1 then 
		return q.Location, l.Damage, q.Count, 2
	elseif n == 2 then
		return q.Location, l.Soul, q.Count, 1
	elseif n == 3 then
		return q.Location, l.EnemyRC, q.Column, 0
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnDiscard, t.Auto, p.HasPrompt, true, p.IsMandatory, false, p.CB, 2, p.SB, 1
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.LastDiscarded() and obj.CanCB(1) and obj.CanSB(2) then
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
		obj.CounterBlast(1)
		obj.SoulBlast(2)
	end
end

function Activate(n)
	if n == 1 then
		local column = obj.SelectColumn()
		obj.Inject(3, q.Column, column) 
		if obj.GetNumberOf(3) > 0 then
			obj.EnemyRearrangeOnBottom(3)
		end
	end
	return 0
end